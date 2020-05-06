using DotSpatial.Projections;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Parcel = MkeAlerts.Web.Models.Data.Places.Parcel;

namespace MkeAlerts.Web.Jobs
{
    public class ImportParcelsJob : ImportShapefileJob<Parcel, string>
    {
        public ImportParcelsJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<ImportParcelsJob> logger, IEntityWriteService<Parcel, string> writeService)
            : base(configuration, signInManager, userManager, mailerService, jobRunService, logger, writeService)
        {
            _shapefileName = configuration.GetValue<string>("ParcelShapefile");
        }

        protected override bool VerifyItem(IShapefileFeature source, Parcel target)
        {
            if (string.IsNullOrEmpty(target.TAXKEY))
                return false;

            target.HouseNumber = ParsingUtilities.ParseInt(target.HOUSENR, 0, true);
            target.HouseNumberHigh = ParsingUtilities.ParseInt(target.HOUSENRHI, 0, true);

            return true;
        }
    }

    public class ImportCommonParcelsJob : ImportShapefileJob<CommonParcel, int>
    {
        public ImportCommonParcelsJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<ImportCommonParcelsJob> logger, IEntityWriteService<CommonParcel, int> writeService)
            : base(configuration, signInManager, userManager, mailerService, jobRunService, logger, writeService)
        {
            _shapefileName = configuration.GetValue<string>("ParcelShapefile");
        }

        protected override bool VerifyItem(IShapefileFeature source, CommonParcel target)
        {
            if (target.MAP_ID == 0)
                return false;

            return true;
        }
    }

    //public class ImportParcelsJob : LoggedJob
    //{
    //    private readonly IEntityWriteService<Parcel, string> _parcelWriteService;
    //    private readonly ICommonParcelService _commonParcelWriteService;
    //    private const int _batchSize = 1000;

    //    public ImportParcelsJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<ImportParcelsJob> logger, IEntityWriteService<Parcel, string> parcelWriteService, ICommonParcelService commonParcelWriteService)
    //        : base(configuration, signInManager, userManager, mailerService, jobRunService, logger)
    //    {
    //        _parcelWriteService = parcelWriteService;
    //        _commonParcelWriteService = commonParcelWriteService;
    //    }

    //    private bool IsFieldValid(IAttributesTable attributes, string name)
    //    {
    //        return attributes.Exists(name) && attributes[name] != null && attributes[name].ToString().Any(x => x != '*');
    //    }

    //    protected override async Task RunInternal()
    //    {
    //        string shapefilePath = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
    //        string fullPath = shapefilePath + "\\" + _configuration.GetValue<string>("ParcelShapefile");

    //        using (ShapeDataReader reader = new ShapeDataReader(fullPath))
    //        {
    //            var mbr = reader.ShapefileBounds;
    //            var result = reader.ReadByMBRFilter(mbr);
    //            var coll = result.GetEnumerator();

    //            List<Parcel> parcels = new List<Parcel>();
    //            List<CommonParcel> commonParcels = new List<CommonParcel>();
    //            List<int> commonParcelIds = new List<int>();

    //            int i = 0;
    //            while (coll.MoveNext())
    //            {
    //                ++i;

    //                Parcel parcel = new Parcel();
    //                bool doAdd = true;

    //                try
    //                {
    //                    ShapefileUtilities.CopyFields(coll.Current, parcel, i, _logger);

    //                    // There are a handful of records without a TAXKEY -- we'll just treat those as invalid, since they may have other weird data
    //                    if (string.IsNullOrEmpty(parcel.TAXKEY))
    //                    {
    //                        _logger.LogTrace("Skipping record " + i.ToString() + " - Invalid TAXKEY");
    //                        ++_failureCount;
    //                        continue;
    //                    }

    //                    // If there's an existing parcel with this TAXKEY, use that one
    //                    Parcel existingParcel = parcels.Where(x => x.TAXKEY == parcel.TAXKEY).SingleOrDefault();
    //                    if (existingParcel != null)
    //                    {
    //                        parcel = existingParcel;
    //                        doAdd = false;
    //                    }

    //                    if (parcel.MAP_ID == 0)
    //                    {
    //                        _logger.LogTrace("Skipping record " + i.ToString() + " - Invalid MAP_ID");
    //                        ++_failureCount;
    //                        continue;
    //                    }

    //                    //if (IsFieldValid(coll.Current.Attributes, "OBJECTID")) parcel.OBJECTID = int.Parse(coll.Current.Attributes["OBJECTID"].ToString());
    //                    //if (IsFieldValid(coll.Current.Attributes, "OVERLAP")) parcel.OVERLAP = int.Parse(coll.Current.Attributes["OVERLAP"].ToString());
    //                    //if (IsFieldValid(coll.Current.Attributes, "RECID")) parcel.RECID = int.Parse(coll.Current.Attributes["RECID"].ToString());
    //                    //if (IsFieldValid(coll.Current.Attributes, "ASSESSEDVA")) parcel.ASSESSEDVA = int.Parse(coll.Current.Attributes["ASSESSEDVA"].ToString());
    //                    //if (IsFieldValid(coll.Current.Attributes, "LANDVALUE")) parcel.LANDVALUE = int.Parse(coll.Current.Attributes["LANDVALUE"].ToString());
    //                    //if (IsFieldValid(coll.Current.Attributes, "IMPVALUE")) parcel.IMPVALUE = int.Parse(coll.Current.Attributes["IMPVALUE"].ToString());
    //                    //if (IsFieldValid(coll.Current.Attributes, "TAX_YR")) parcel.TAX_YR = int.Parse(coll.Current.Attributes["TAX_YR"].ToString());
    //                    //if (IsFieldValid(coll.Current.Attributes, "FAIR_MKT_V")) parcel.FAIR_MKT_V = int.Parse(coll.Current.Attributes["FAIR_MKT_V"].ToString());

    //                    //if (IsFieldValid(coll.Current.Attributes, "ACRES")) parcel.ACRES = double.Parse(coll.Current.Attributes["ACRES"].ToString());
    //                    //if (IsFieldValid(coll.Current.Attributes, "GROSS_TAX")) parcel.GROSS_TAX = double.Parse(coll.Current.Attributes["GROSS_TAX"].ToString());
    //                    //if (IsFieldValid(coll.Current.Attributes, "NET_TAX")) parcel.NET_TAX = double.Parse(coll.Current.Attributes["NET_TAX"].ToString());
    //                    //if (IsFieldValid(coll.Current.Attributes, "GIS_ACRES")) parcel.GIS_ACRES = double.Parse(coll.Current.Attributes["GIS_ACRES"].ToString());

    //                    //if (IsFieldValid(coll.Current.Attributes, "PARCEL_KEY")) parcel.PARCEL_KEY = coll.Current.Attributes["PARCEL_KEY"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "PARCEL_DES")) parcel.PARCEL_DES = coll.Current.Attributes["PARCEL_DES"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "MCD")) parcel.MCD = coll.Current.Attributes["MCD"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "SOURCE")) parcel.SOURCE = coll.Current.Attributes["SOURCE"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "COMMENT")) parcel.COMMENT = coll.Current.Attributes["COMMENT"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "RECSOURCE")) parcel.RECSOURCE = coll.Current.Attributes["RECSOURCE"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "MUNINAME")) parcel.MUNINAME = coll.Current.Attributes["MUNINAME"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "PARCELNO")) parcel.PARCELNO = coll.Current.Attributes["PARCELNO"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "OWNERNAME1")) parcel.OWNERNAME1 = coll.Current.Attributes["OWNERNAME1"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "OWNERNAME2")) parcel.OWNERNAME2 = coll.Current.Attributes["OWNERNAME2"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "OWNERNAME3")) parcel.OWNERNAME3 = coll.Current.Attributes["OWNERNAME3"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "OWNERADDR")) parcel.OWNERADDR = coll.Current.Attributes["OWNERADDR"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "OWNERCTYST")) parcel.OWNERCTYST = coll.Current.Attributes["OWNERCTYST"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "OWNERZIP")) parcel.OWNERZIP = coll.Current.Attributes["OWNERZIP"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "HOUSENR")) parcel.HOUSENR = coll.Current.Attributes["HOUSENR"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "HOUSENRHI")) parcel.HOUSENRHI = coll.Current.Attributes["HOUSENRHI"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "HOUSENRSFX")) parcel.HOUSENRSFX = coll.Current.Attributes["HOUSENRSFX"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "STREETDIR")) parcel.STREETDIR = coll.Current.Attributes["STREETDIR"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "STREETNAME")) parcel.STREETNAME = coll.Current.Attributes["STREETNAME"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "STREETTYPE")) parcel.STREETTYPE = coll.Current.Attributes["STREETTYPE"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "SUFFIXDIR")) parcel.SUFFIXDIR = coll.Current.Attributes["SUFFIXDIR"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "UNITNUMBER")) parcel.UNITNUMBER = coll.Current.Attributes["UNITNUMBER"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "POSTOFFICE")) parcel.POSTOFFICE = coll.Current.Attributes["POSTOFFICE"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "LEGALDESCR")) parcel.LEGALDESCR = coll.Current.Attributes["LEGALDESCR"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "CONDO_NAME")) parcel.CONDO_NAME = coll.Current.Attributes["CONDO_NAME"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "UNIT_TYPE")) parcel.UNIT_TYPE = coll.Current.Attributes["UNIT_TYPE"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "CLASS")) parcel.CLASS = coll.Current.Attributes["CLASS"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "CODE")) parcel.CODE = coll.Current.Attributes["CODE"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "DESCRIPTIO")) parcel.DESCRIPTIO = coll.Current.Attributes["DESCRIPTIO"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "ZONING_COD")) parcel.ZONING_COD = coll.Current.Attributes["ZONING_COD"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "ZONING_DES")) parcel.ZONING_DES = coll.Current.Attributes["ZONING_DES"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "ZONING_URL")) parcel.ZONING_URL = coll.Current.Attributes["ZONING_URL"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "EXM_TYP")) parcel.EXM_TYP = coll.Current.Attributes["EXM_TYP"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "EXM_TYP_DE")) parcel.EXM_TYP_DE = coll.Current.Attributes["EXM_TYP_DE"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "EXM_CLASS_")) parcel.EXM_CLASS_ = coll.Current.Attributes["EXM_CLASS_"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "TAX_INFO_U")) parcel.TAX_INFO_U = coll.Current.Attributes["TAX_INFO_U"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "ASSESSMENT")) parcel.ASSESSMENT = coll.Current.Attributes["ASSESSMENT"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "PARCEL_TYP")) parcel.PARCEL_TYP = coll.Current.Attributes["PARCEL_TYP"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "SCHOOL_DIS")) parcel.SCHOOL_DIS = coll.Current.Attributes["SCHOOL_DIS"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "SCHOOL_ID")) parcel.SCHOOL_ID = coll.Current.Attributes["SCHOOL_ID"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "PAR_ZIP")) parcel.PAR_ZIP = coll.Current.Attributes["PAR_ZIP"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "PAR_ZIP_EX")) parcel.PAR_ZIP_EX = coll.Current.Attributes["PAR_ZIP_EX"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "ADDRESS")) parcel.ADDRESS = coll.Current.Attributes["ADDRESS"].ToString();
    //                    //if (IsFieldValid(coll.Current.Attributes, "DWELLING_C")) parcel.DWELLING_C = coll.Current.Attributes["DWELLING_C"].ToString();

    //                    CommonParcel commonParcel = commonParcels.Where(x => x.MAP_ID == parcel.MAP_ID).SingleOrDefault();

    //                    if (commonParcel == null)
    //                    {
    //                        //GeometryFactory geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
    //                        //Polygon transformedGeometry = null;

    //                        //if (coll.Current.Geometry.GeometryType == "Polygon")
    //                        //{
    //                        //    transformedGeometry = (Polygon)geometryFactory.CreatePolygon(coll.Current.Geometry.Coordinates);
    //                        //}
    //                        //else if (coll.Current.Geometry.GeometryType == "MultiPolygon")
    //                        //{
    //                        //    List<Polygon> polygons = new List<Polygon>();
    //                        //    foreach (Polygon polygon in ((MultiPolygon)coll.Current.Geometry).Geometries)
    //                        //    {
    //                        //        polygons.Add((Polygon)geometryFactory.CreatePolygon(polygon.Coordinates));
    //                        //    }
    //                        //    geometryFactory.CreateMultiPolygon(polygons.ToArray());
    //                        //}

    //                        //// https://gis.stackexchange.com/questions/289545/using-sqlgeometry-makevalid-to-get-a-counter-clockwise-polygon-in-sql-server
    //                        //if (transformedGeometry != null && !transformedGeometry.Shell.IsCCW)
    //                        //    transformedGeometry = (Polygon)transformedGeometry.Reverse();

    //                        //if (transformedGeometry == null)
    //                        //{
    //                        //    _logger.LogTrace("Skipping record " + i.ToString() + " - No transformed geometry");

    //                        //    ++_failureCount;
    //                        //    continue;
    //                        //}

    //                        commonParcel = new CommonParcel()
    //                        {
    //                            MAP_ID = parcel.MAP_ID
    //                        };

    //                        if (!ShapefileUtilities.CopyFields(coll.Current, commonParcel, i, _logger))
    //                        {
    //                            ++_failureCount;
    //                            continue;
    //                        }
    //                        //GeographicUtilities.SetBounds(commonParcel, commonParcel.Outline);
    //                        commonParcels.Add(commonParcel);
    //                    }

    //                    if (doAdd)
    //                        parcels.Add(parcel);

    //                    if (i % _batchSize == 0)
    //                    {
    //                        await MergeItems(commonParcels, parcels, i);
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                    _logger.LogError(ex, "Error importing parcel: " + parcel.TAXKEY);
    //                }
    //            }

    //            await MergeItems(commonParcels, parcels, i);
    //        }
    //    }

    //    private async Task MergeItems(List<CommonParcel> commonParcels, List<Parcel> parcels, int i)
    //    {
    //        ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

    //        try
    //        {
    //            Tuple<IEnumerable<CommonParcel>, IEnumerable<CommonParcel>> commonParcelResults = await _commonParcelWriteService.BulkCreate(claimsPrincipal, commonParcels, false);
    //            commonParcels.Clear();

    //            Tuple<IEnumerable<Parcel>, IEnumerable<Parcel>> parcelResults = await _parcelWriteService.BulkCreate(claimsPrincipal, parcels);
    //            _successCount += parcelResults.Item1.Count();
    //            _failureCount += parcelResults.Item2.Count();
    //            parcels.Clear();

    //            _logger.LogDebug("Bulk inserted items at mod " + i.ToString());
    //        }
    //        catch (Exception ex)
    //        {
    //            _failureCount += parcels.Count;

    //            _logger.LogError(ex, "Error bulk inserting items at mod " + i.ToString());
    //        }

    //    }
    //}
}
