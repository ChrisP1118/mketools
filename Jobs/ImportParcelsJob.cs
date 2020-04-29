using DotSpatial.Projections;
using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;
using GeoAPI.Geometries;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Services;
using MkeAlerts.Web.Services.Data;
using MkeAlerts.Web.Services.Data.Interfaces;
using MkeAlerts.Web.Services.Functional;
using MkeAlerts.Web.Utilities;
using NetTopologySuite;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.IO.ShapeFile.Extended;
using NetTopologySuite.IO.ShapeFile.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Parcel = MkeAlerts.Web.Models.Data.Places.Parcel;

namespace MkeAlerts.Web.Jobs
{
    public class ImportParcelsJob : Job
    {
        private readonly ILogger<ImportParcelsJob> _logger;
        private readonly IEntityWriteService<Parcel, string> _parcelWriteService;
        private readonly ICommonParcelService _commonParcelWriteService;

        public ImportParcelsJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, ILogger<ImportParcelsJob> logger, IEntityWriteService<Parcel, string> parcelWriteService, ICommonParcelService commonParcelWriteService)
            : base(configuration, signInManager, userManager, mailerService)
        {
            _logger = logger;
            _parcelWriteService = parcelWriteService;
            _commonParcelWriteService = commonParcelWriteService;
        }

        private bool IsFieldValid(IAttributesTable attributes, string name)
        {
            return attributes.Exists(name) && attributes[name] != null;
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task Run()
        {
            try
            {
                _logger.LogInformation("Starting job");

                string fileName = await PackageUtilities.DownloadPackageFile(_logger, "parcel-outlines", null, "parcelpolygon");

                _logger.LogDebug("Download complete: " + fileName);

                string folderName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(folderName);

                ZipFile.ExtractToDirectory(fileName, folderName);
                _logger.LogDebug(fileName + " unzipped to " + folderName);

                string shapeFileName = folderName + "\\ParcelPolygonTax.shp";
                string projectionFileName = folderName + "\\ParcelPolygonTax.prj";

                ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

                var projectionInfo = ProjectionInfo.Open(projectionFileName);

                int success = 0;
                int failure = 0;

                using (ShapeDataReader reader = new ShapeDataReader(shapeFileName))
                {
                    var mbr = reader.ShapefileBounds;
                    var result = reader.ReadByMBRFilter(mbr);
                    var coll = result.GetEnumerator();

                    List<Parcel> parcels = new List<Parcel>();
                    List<CommonParcel> commonParcels = new List<CommonParcel>();
                    List<CommonParcel> previousCommonParcels = new List<CommonParcel>();

                    int i = 0;
                    while (coll.MoveNext())
                    {
                        ++i;

                        try
                        {
                            Parcel parcel = new Parcel();
                            parcel.Taxkey = coll.Current.Attributes["Taxkey"].ToString();

                            // There are a handful of records without a TAXKEY -- we'll just treat those as invalid, since they may have other weird data
                            if (string.IsNullOrEmpty(parcel.Taxkey) || parcel.Taxkey == "<Null>")
                            {
                                _logger.LogTrace("Skipping record " + i.ToString() + " - Invalid TAXKEY");
                                ++failure;
                                continue;
                            }

                            //if (coll.Current.Attributes["ID"] != null) parcel.ID = coll.Current.Attributes["ID"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "FK_Parcel")) parcel.FK_Parcel = int.Parse(coll.Current.Attributes["FK_Parcel"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "FK_LandUse")) parcel.FK_LandUse = int.Parse(coll.Current.Attributes["FK_LandUse"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "FK_Naics")) parcel.FK_Naics = int.Parse(coll.Current.Attributes["FK_Naics"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "FK_Histori")) parcel.FK_Histori = int.Parse(coll.Current.Attributes["FK_Histori"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "FK_Zoning")) parcel.FK_Zoning = coll.Current.Attributes["FK_Zoning"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "ZoningCFN")) parcel.ZoningCFN = coll.Current.Attributes["ZoningCFN"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "ComDiv")) parcel.ComDiv = int.Parse(coll.Current.Attributes["ComDiv"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "Source")) parcel.Source = int.Parse(coll.Current.Attributes["Source"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "RecordDate")) parcel.RecordDate = DateTime.Parse(coll.Current.Attributes["RecordDate"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "Comments")) parcel.Comments = coll.Current.Attributes["Comments"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "CondoName")) parcel.CondoName = coll.Current.Attributes["CondoName"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "CondoType")) parcel.CondoType = coll.Current.Attributes["CondoType"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "CondoUnitT")) parcel.CondoUnitT = coll.Current.Attributes["CondoUnitT"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "UpdatedDat")) parcel.UpdatedDat = DateTime.Parse(coll.Current.Attributes["UpdatedDat"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "ActivatedD")) parcel.ActivatedD = DateTime.Parse(coll.Current.Attributes["ActivatedD"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "InactiveFl")) parcel.InactiveFl = int.Parse(coll.Current.Attributes["InactiveFl"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "DistrictNa")) parcel.DistrictNa = coll.Current.Attributes["DistrictNa"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "Neighborho")) parcel.Neighborho = coll.Current.Attributes["Neighborho"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "PlatPage")) parcel.PlatPage = coll.Current.Attributes["PlatPage"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "StreetNumb")) parcel.StreetNumb = int.Parse(coll.Current.Attributes["StreetNumb"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "AlternateS")) parcel.AlternateS = int.Parse(coll.Current.Attributes["AlternateS"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "StreetNu_1")) parcel.StreetNu_1 = coll.Current.Attributes["StreetNu_1"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "StreetDire")) parcel.StreetDire = coll.Current.Attributes["StreetDire"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "StreetName")) parcel.StreetName = coll.Current.Attributes["StreetName"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "STTYPE")) parcel.STTYPE = coll.Current.Attributes["STTYPE"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "StreetType")) parcel.STTYPE = coll.Current.Attributes["StreetType"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "LandUse")) parcel.LandUse = coll.Current.Attributes["LandUse"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "PropertySt")) parcel.PropertySt = coll.Current.Attributes["PropertySt"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "PrimaryJur")) parcel.PrimaryJur = coll.Current.Attributes["PrimaryJur"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "PreviousYe")) parcel.PreviousYe = coll.Current.Attributes["PreviousYe"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "Previous_1")) parcel.Previous_1 = coll.Current.Attributes["Previous_1"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "CurrentYea")) parcel.CurrentYea = coll.Current.Attributes["CurrentYea"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "TotalLandV")) parcel.TotalLandV = coll.Current.Attributes["TotalLandV"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "TotalYardI")) parcel.TotalYardI = coll.Current.Attributes["TotalYardI"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "TotalAsses")) parcel.TotalAsses = coll.Current.Attributes["TotalAsses"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "TotalLandE")) parcel.TotalLandE = coll.Current.Attributes["TotalLandE"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "TotalYar_1")) parcel.TotalYar_1 = coll.Current.Attributes["TotalYar_1"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "TotalAss_1")) parcel.TotalAss_1 = coll.Current.Attributes["TotalAss_1"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "PrevNonExe")) parcel.PrevNonExe = coll.Current.Attributes["PrevNonExe"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "PreviousEx")) parcel.PreviousEx = coll.Current.Attributes["PreviousEx"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "TotalPrevN")) parcel.TotalPrevN = int.Parse(coll.Current.Attributes["TotalPrevN"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "TotalPre_1")) parcel.TotalPre_1 = int.Parse(coll.Current.Attributes["TotalPre_1"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "TotaPrevNo")) parcel.TotaPrevNo = int.Parse(coll.Current.Attributes["TotaPrevNo"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "TotalPrevL")) parcel.TotalPrevL = int.Parse(coll.Current.Attributes["TotalPrevL"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "TotalPrevY")) parcel.TotalPrevY = int.Parse(coll.Current.Attributes["TotalPrevY"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "TotalPrevE")) parcel.TotalPrevE = int.Parse(coll.Current.Attributes["TotalPrevE"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "AsmtChange")) parcel.AsmtChange = coll.Current.Attributes["AsmtChange"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "AsmtChan_1")) parcel.AsmtChan_1 = coll.Current.Attributes["AsmtChan_1"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "SaleDate")) parcel.SaleDate = DateTime.Parse(coll.Current.Attributes["SaleDate"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "Deed")) parcel.Deed = coll.Current.Attributes["Deed"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "CONVEY_FEE")) parcel.CONVEY_FEE = float.Parse(coll.Current.Attributes["CONVEY_FEE"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "Owner1")) parcel.Owner1 = coll.Current.Attributes["Owner1"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "Owner2")) parcel.Owner2 = coll.Current.Attributes["Owner2"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "Owner3")) parcel.Owner3 = coll.Current.Attributes["Owner3"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "OwnerBilli")) parcel.OwnerBilli = coll.Current.Attributes["OwnerBilli"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "OwnerCityS")) parcel.OwnerCityS = coll.Current.Attributes["OwnerCityS"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "OwnerZipCo")) parcel.OwnerZipCo = coll.Current.Attributes["OwnerZipCo"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "OwnerNameC")) parcel.OwnerNameC = DateTime.Parse(coll.Current.Attributes["OwnerNameC"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "BuildingTy")) parcel.BuildingTy = coll.Current.Attributes["BuildingTy"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "Commercial")) parcel.Commercial = int.Parse(coll.Current.Attributes["Commercial"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "Residentia")) parcel.Residentia = int.Parse(coll.Current.Attributes["Residentia"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "NR_STORIES")) parcel.NR_STORIES = coll.Current.Attributes["NR_STORIES"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "YearBuilt")) parcel.YearBuilt = int.Parse(coll.Current.Attributes["YearBuilt"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "NumberOfFi")) parcel.NumberOfFi = int.Parse(coll.Current.Attributes["NumberOfFi"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "PercentAir")) parcel.PercentAir = float.Parse(coll.Current.Attributes["PercentAir"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "NumberOfFu")) parcel.NumberOfFu = int.Parse(coll.Current.Attributes["NumberOfFu"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "NumberOfHa")) parcel.NumberOfHa = int.Parse(coll.Current.Attributes["NumberOfHa"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "NumberOfRo")) parcel.NumberOfRo = int.Parse(coll.Current.Attributes["NumberOfRo"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "NumberOfBe")) parcel.NumberOfBe = int.Parse(coll.Current.Attributes["NumberOfBe"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "ATTIC")) parcel.ATTIC = coll.Current.Attributes["ATTIC"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "BASEMENT")) parcel.BASEMENT = coll.Current.Attributes["BASEMENT"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "BLDG_AREA")) parcel.BLDG_AREA = float.Parse(coll.Current.Attributes["BLDG_AREA"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "ParkingTyp")) parcel.ParkingTyp = coll.Current.Attributes["ParkingTyp"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "Calculated")) parcel.Calculated = float.Parse(coll.Current.Attributes["Calculated"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "Calculat_1")) parcel.Calculat_1 = float.Parse(coll.Current.Attributes["Calculat_1"].ToString());
                            //if (IsFieldValid(coll.Current.Attributes, "Taxkey")) parcel.Taxkey = coll.Current.Attributes["Taxkey"].ToString();
                            if (IsFieldValid(coll.Current.Attributes, "ParcelType")) parcel.ParcelType = int.Parse(coll.Current.Attributes["ParcelType"].ToString());
                            if (IsFieldValid(coll.Current.Attributes, "ParcelActi")) parcel.ParcelActi = DateTime.Parse(coll.Current.Attributes["ParcelActi"].ToString());


                            IGeometryFactory geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

                            //IPoint projectedCentroid = GeographicUtilities.ReprojectCoordinates(projectionInfo, coll.Current.Geometry.Centroid);
                            //IPoint transformedCentroid = geometryFactory.CreatePoint(new Coordinate(projectedCentroid.X, projectedCentroid.Y));
                            //location.Centroid = transformedCentroid;

                            Polygon transformedGeometry = null;

                            if (coll.Current.Geometry.GeometryType == "Polygon")
                            {
                                Coordinate[] projectedCoordinates = GeographicUtilities.ReprojectCoordinates(projectionInfo, coll.Current.Geometry.Coordinates);
                                transformedGeometry = (Polygon)geometryFactory.CreatePolygon(projectedCoordinates);
                            }
                            else if (coll.Current.Geometry.GeometryType == "MultiPolygon")
                            {
                                List<IPolygon> polygons = new List<IPolygon>();
                                foreach (IPolygon polygon in ((MultiPolygon)coll.Current.Geometry).Geometries)
                                {
                                    Coordinate[] projectedCoordinates = GeographicUtilities.ReprojectCoordinates(projectionInfo, polygon.Coordinates);
                                    polygons.Add((Polygon)geometryFactory.CreatePolygon(projectedCoordinates));
                                }
                                geometryFactory.CreateMultiPolygon(polygons.ToArray());
                            }

                            // https://gis.stackexchange.com/questions/289545/using-sqlgeometry-makevalid-to-get-a-counter-clockwise-polygon-in-sql-server
                            if (transformedGeometry != null && !transformedGeometry.Shell.IsCCW)
                                transformedGeometry = (Polygon)transformedGeometry.Reverse();

                            if (transformedGeometry == null)
                            {
                                _logger.LogTrace("Skipping record " + i.ToString() + " - No transformed geometry");

                                ++failure;
                                continue;
                            }

                            CommonParcel commonParcel = commonParcels.Where(x => x.Outline.EqualsTopologically(transformedGeometry)).FirstOrDefault();
                            if (commonParcel == null)
                            {
                                commonParcel = previousCommonParcels.Where(x => x.Outline.EqualsTopologically(transformedGeometry)).FirstOrDefault();

                                if (commonParcel == null)
                                {
                                    commonParcel = new CommonParcel()
                                    {
                                        Id = Guid.NewGuid(),
                                        Outline = transformedGeometry
                                    };
                                    GeographicUtilities.SetBounds(commonParcel, commonParcel.Outline);
                                    commonParcels.Add(commonParcel);
                                }
                            }

                            parcel.CommonParcelId = commonParcel.Id;

                            //parcel.Outline = transformedGeometry;
                            //GeographicUtilities.SetBounds(parcel, parcel.Outline);

                            parcels.Add(parcel);

                            if (i % 100 == 0)
                            {
                                try
                                {
                                    Tuple<IEnumerable<CommonParcel>, IEnumerable<CommonParcel>> results3 = await _commonParcelWriteService.BulkCreate(claimsPrincipal, commonParcels, false);
                                    previousCommonParcels = new List<CommonParcel>(commonParcels.ToList());
                                    commonParcels.Clear();

                                    Tuple<IEnumerable<Parcel>, IEnumerable<Parcel>> results1 = await _parcelWriteService.BulkCreate(claimsPrincipal, parcels, false);
                                    success += results1.Item1.Count();
                                    failure += results1.Item2.Count();
                                    parcels.Clear();

                                    _logger.LogDebug("Bulk inserted items at mod " + i.ToString());
                                }
                                catch (Exception ex)
                                {
                                    failure += parcels.Count;

                                    _logger.LogError(ex, "Error bulk inserting items at mod " + i.ToString());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error importing Location");
                        }
                    }

                    try
                    {
                        Tuple<IEnumerable<CommonParcel>, IEnumerable<CommonParcel>> results4 = await _commonParcelWriteService.BulkCreate(claimsPrincipal, commonParcels, false);

                        Tuple<IEnumerable<Parcel>, IEnumerable<Parcel>> results2 = await _parcelWriteService.BulkCreate(claimsPrincipal, parcels, false);
                        success += results2.Item1.Count();
                        failure += results2.Item2.Count();

                        _logger.LogDebug("Bulk inserted items at mod " + i.ToString());
                    }
                    catch (Exception ex)
                    {
                        failure += parcels.Count;

                        _logger.LogError(ex, "Error bulk inserting items at mod " + i.ToString());
                    }
                }

                _logger.LogInformation("Import results: " + success.ToString() + " success, " + failure.ToString() + " failure");

                await _commonParcelWriteService.RemoveDuplicates();

                _logger.LogInformation("Removed duplicates");

                foreach (string file in Directory.EnumerateFiles(folderName))
                    File.Delete(file);
                Directory.Delete(folderName);
                File.Delete(fileName);

                _logger.LogInformation("Finishing job");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error running " + this.GetType().Name + ": " + ex);

                await _mailerService.SendAdminAlert("Error running " + this.GetType().Name, ex.Message);
            }
        }
    }
}
