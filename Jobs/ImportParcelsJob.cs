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
                            if (coll.Current.Attributes["FK_Parcel"] != null) parcel.FK_Parcel = int.Parse(coll.Current.Attributes["FK_Parcel"].ToString());
                            if (coll.Current.Attributes["FK_LandUse"] != null) parcel.FK_LandUse = int.Parse(coll.Current.Attributes["FK_LandUse"].ToString());
                            if (coll.Current.Attributes["FK_Naics"] != null) parcel.FK_Naics = int.Parse(coll.Current.Attributes["FK_Naics"].ToString());
                            if (coll.Current.Attributes["FK_Histori"] != null) parcel.FK_Histori = int.Parse(coll.Current.Attributes["FK_Histori"].ToString());
                            if (coll.Current.Attributes["FK_Zoning"] != null) parcel.FK_Zoning = coll.Current.Attributes["FK_Zoning"].ToString();
                            if (coll.Current.Attributes["ZoningCFN"] != null) parcel.ZoningCFN = coll.Current.Attributes["ZoningCFN"].ToString();
                            if (coll.Current.Attributes["ComDiv"] != null) parcel.ComDiv = int.Parse(coll.Current.Attributes["ComDiv"].ToString());
                            if (coll.Current.Attributes["Source"] != null) parcel.Source = int.Parse(coll.Current.Attributes["Source"].ToString());
                            if (coll.Current.Attributes["RecordDate"] != null) parcel.RecordDate = DateTime.Parse(coll.Current.Attributes["RecordDate"].ToString());
                            if (coll.Current.Attributes["Comments"] != null) parcel.Comments = coll.Current.Attributes["Comments"].ToString();
                            if (coll.Current.Attributes["CondoName"] != null) parcel.CondoName = coll.Current.Attributes["CondoName"].ToString();
                            if (coll.Current.Attributes["CondoType"] != null) parcel.CondoType = coll.Current.Attributes["CondoType"].ToString();
                            if (coll.Current.Attributes["CondoUnitT"] != null) parcel.CondoUnitT = coll.Current.Attributes["CondoUnitT"].ToString();
                            if (coll.Current.Attributes["UpdatedDat"] != null) parcel.UpdatedDat = DateTime.Parse(coll.Current.Attributes["UpdatedDat"].ToString());
                            if (coll.Current.Attributes["ActivatedD"] != null) parcel.ActivatedD = DateTime.Parse(coll.Current.Attributes["ActivatedD"].ToString());
                            if (coll.Current.Attributes["InactiveFl"] != null) parcel.InactiveFl = int.Parse(coll.Current.Attributes["InactiveFl"].ToString());
                            if (coll.Current.Attributes["DistrictNa"] != null) parcel.DistrictNa = coll.Current.Attributes["DistrictNa"].ToString();
                            if (coll.Current.Attributes["Neighborho"] != null) parcel.Neighborho = coll.Current.Attributes["Neighborho"].ToString();
                            if (coll.Current.Attributes["PlatPage"] != null) parcel.PlatPage = coll.Current.Attributes["PlatPage"].ToString();
                            if (coll.Current.Attributes["StreetNumb"] != null) parcel.StreetNumb = int.Parse(coll.Current.Attributes["StreetNumb"].ToString());
                            if (coll.Current.Attributes["AlternateS"] != null) parcel.AlternateS = int.Parse(coll.Current.Attributes["AlternateS"].ToString());
                            if (coll.Current.Attributes["StreetNu_1"] != null) parcel.StreetNu_1 = coll.Current.Attributes["StreetNu_1"].ToString();
                            if (coll.Current.Attributes["StreetDire"] != null) parcel.StreetDire = coll.Current.Attributes["StreetDire"].ToString();
                            if (coll.Current.Attributes["StreetName"] != null) parcel.StreetName = coll.Current.Attributes["StreetName"].ToString();
                            if (coll.Current.Attributes["STTYPE"] != null) parcel.STTYPE = coll.Current.Attributes["STTYPE"].ToString();
                            if (coll.Current.Attributes["LandUse"] != null) parcel.LandUse = coll.Current.Attributes["LandUse"].ToString();
                            if (coll.Current.Attributes["PropertySt"] != null) parcel.PropertySt = coll.Current.Attributes["PropertySt"].ToString();
                            if (coll.Current.Attributes["PrimaryJur"] != null) parcel.PrimaryJur = coll.Current.Attributes["PrimaryJur"].ToString();
                            if (coll.Current.Attributes["PreviousYe"] != null) parcel.PreviousYe = coll.Current.Attributes["PreviousYe"].ToString();
                            if (coll.Current.Attributes["Previous_1"] != null) parcel.Previous_1 = coll.Current.Attributes["Previous_1"].ToString();
                            if (coll.Current.Attributes["CurrentYea"] != null) parcel.CurrentYea = coll.Current.Attributes["CurrentYea"].ToString();
                            if (coll.Current.Attributes["TotalLandV"] != null) parcel.TotalLandV = coll.Current.Attributes["TotalLandV"].ToString();
                            if (coll.Current.Attributes["TotalYardI"] != null) parcel.TotalYardI = coll.Current.Attributes["TotalYardI"].ToString();
                            if (coll.Current.Attributes["TotalAsses"] != null) parcel.TotalAsses = coll.Current.Attributes["TotalAsses"].ToString();
                            if (coll.Current.Attributes["TotalLandE"] != null) parcel.TotalLandE = coll.Current.Attributes["TotalLandE"].ToString();
                            if (coll.Current.Attributes["TotalYar_1"] != null) parcel.TotalYar_1 = coll.Current.Attributes["TotalYar_1"].ToString();
                            if (coll.Current.Attributes["TotalAss_1"] != null) parcel.TotalAss_1 = coll.Current.Attributes["TotalAss_1"].ToString();
                            if (coll.Current.Attributes["PrevNonExe"] != null) parcel.PrevNonExe = coll.Current.Attributes["PrevNonExe"].ToString();
                            if (coll.Current.Attributes["PreviousEx"] != null) parcel.PreviousEx = coll.Current.Attributes["PreviousEx"].ToString();
                            if (coll.Current.Attributes["TotalPrevN"] != null) parcel.TotalPrevN = int.Parse(coll.Current.Attributes["TotalPrevN"].ToString());
                            if (coll.Current.Attributes["TotalPre_1"] != null) parcel.TotalPre_1 = int.Parse(coll.Current.Attributes["TotalPre_1"].ToString());
                            if (coll.Current.Attributes["TotaPrevNo"] != null) parcel.TotaPrevNo = int.Parse(coll.Current.Attributes["TotaPrevNo"].ToString());
                            if (coll.Current.Attributes["TotalPrevL"] != null) parcel.TotalPrevL = int.Parse(coll.Current.Attributes["TotalPrevL"].ToString());
                            if (coll.Current.Attributes["TotalPrevY"] != null) parcel.TotalPrevY = int.Parse(coll.Current.Attributes["TotalPrevY"].ToString());
                            if (coll.Current.Attributes["TotalPrevE"] != null) parcel.TotalPrevE = int.Parse(coll.Current.Attributes["TotalPrevE"].ToString());
                            if (coll.Current.Attributes["AsmtChange"] != null) parcel.AsmtChange = coll.Current.Attributes["AsmtChange"].ToString();
                            if (coll.Current.Attributes["AsmtChan_1"] != null) parcel.AsmtChan_1 = coll.Current.Attributes["AsmtChan_1"].ToString();
                            if (coll.Current.Attributes["SaleDate"] != null) parcel.SaleDate = DateTime.Parse(coll.Current.Attributes["SaleDate"].ToString());
                            if (coll.Current.Attributes["Deed"] != null) parcel.Deed = coll.Current.Attributes["Deed"].ToString();
                            if (coll.Current.Attributes["CONVEY_FEE"] != null) parcel.CONVEY_FEE = float.Parse(coll.Current.Attributes["CONVEY_FEE"].ToString());
                            if (coll.Current.Attributes["Owner1"] != null) parcel.Owner1 = coll.Current.Attributes["Owner1"].ToString();
                            if (coll.Current.Attributes["Owner2"] != null) parcel.Owner2 = coll.Current.Attributes["Owner2"].ToString();
                            if (coll.Current.Attributes["Owner3"] != null) parcel.Owner3 = coll.Current.Attributes["Owner3"].ToString();
                            if (coll.Current.Attributes["OwnerBilli"] != null) parcel.OwnerBilli = coll.Current.Attributes["OwnerBilli"].ToString();
                            if (coll.Current.Attributes["OwnerCityS"] != null) parcel.OwnerCityS = coll.Current.Attributes["OwnerCityS"].ToString();
                            if (coll.Current.Attributes["OwnerZipCo"] != null) parcel.OwnerZipCo = coll.Current.Attributes["OwnerZipCo"].ToString();
                            if (coll.Current.Attributes["OwnerNameC"] != null) parcel.OwnerNameC = DateTime.Parse(coll.Current.Attributes["OwnerNameC"].ToString());
                            if (coll.Current.Attributes["BuildingTy"] != null) parcel.BuildingTy = coll.Current.Attributes["BuildingTy"].ToString();
                            if (coll.Current.Attributes["Commercial"] != null) parcel.Commercial = int.Parse(coll.Current.Attributes["Commercial"].ToString());
                            if (coll.Current.Attributes["Residentia"] != null) parcel.Residentia = int.Parse(coll.Current.Attributes["Residentia"].ToString());
                            if (coll.Current.Attributes["NR_STORIES"] != null) parcel.NR_STORIES = coll.Current.Attributes["NR_STORIES"].ToString();
                            if (coll.Current.Attributes["YearBuilt"] != null) parcel.YearBuilt = int.Parse(coll.Current.Attributes["YearBuilt"].ToString());
                            if (coll.Current.Attributes["NumberOfFi"] != null) parcel.NumberOfFi = int.Parse(coll.Current.Attributes["NumberOfFi"].ToString());
                            if (coll.Current.Attributes["PercentAir"] != null) parcel.PercentAir = float.Parse(coll.Current.Attributes["PercentAir"].ToString());
                            if (coll.Current.Attributes["NumberOfFu"] != null) parcel.NumberOfFu = int.Parse(coll.Current.Attributes["NumberOfFu"].ToString());
                            if (coll.Current.Attributes["NumberOfHa"] != null) parcel.NumberOfHa = int.Parse(coll.Current.Attributes["NumberOfHa"].ToString());
                            if (coll.Current.Attributes["NumberOfRo"] != null) parcel.NumberOfRo = int.Parse(coll.Current.Attributes["NumberOfRo"].ToString());
                            if (coll.Current.Attributes["NumberOfBe"] != null) parcel.NumberOfBe = int.Parse(coll.Current.Attributes["NumberOfBe"].ToString());
                            if (coll.Current.Attributes["ATTIC"] != null) parcel.ATTIC = coll.Current.Attributes["ATTIC"].ToString();
                            if (coll.Current.Attributes["BASEMENT"] != null) parcel.BASEMENT = coll.Current.Attributes["BASEMENT"].ToString();
                            if (coll.Current.Attributes["BLDG_AREA"] != null) parcel.BLDG_AREA = float.Parse(coll.Current.Attributes["BLDG_AREA"].ToString());
                            if (coll.Current.Attributes["ParkingTyp"] != null) parcel.ParkingTyp = coll.Current.Attributes["ParkingTyp"].ToString();
                            if (coll.Current.Attributes["Calculated"] != null) parcel.Calculated = float.Parse(coll.Current.Attributes["Calculated"].ToString());
                            if (coll.Current.Attributes["Calculat_1"] != null) parcel.Calculat_1 = float.Parse(coll.Current.Attributes["Calculat_1"].ToString());
                            //if (coll.Current.Attributes["Taxkey"] != null) parcel.Taxkey = coll.Current.Attributes["Taxkey"].ToString();
                            if (coll.Current.Attributes["ParcelType"] != null) parcel.ParcelType = int.Parse(coll.Current.Attributes["ParcelType"].ToString());
                            if (coll.Current.Attributes["ParcelActi"] != null) parcel.ParcelActi = DateTime.Parse(coll.Current.Attributes["ParcelActi"].ToString());


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
