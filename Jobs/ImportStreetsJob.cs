using DotSpatial.Projections;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Services;
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

namespace MkeAlerts.Web.Jobs
{
    public class ImportStreetsJob : Job
    {
        private readonly ILogger<ImportStreetsJob> _logger;
        private readonly IEntityWriteService<Street, string> _streetWriteService;

        public ImportStreetsJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, ILogger<ImportStreetsJob> logger, IEntityWriteService<Street, string> streetWriteService)
            : base(configuration, signInManager, userManager, mailerService)
        {
            _logger = logger;
            _streetWriteService = streetWriteService;
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task Run()
        {
            try
            {
                _logger.LogInformation("Starting job");

                string fileName = await PackageUtilities.DownloadPackageFile(_logger, "streets", "ZIP");

                _logger.LogDebug("Download complete: " + fileName);

                string folderName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(folderName);

                ZipFile.ExtractToDirectory(fileName, folderName);
                _logger.LogDebug(fileName + " unzipped to " + folderName);

                string shapeFileName = folderName + "\\dime.shp";
                string projectionFileName = folderName + "\\dime.prj";

                ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

                var projectionInfo = ProjectionInfo.Open(projectionFileName);

                int success = 0;
                int failure = 0;

                using (ShapeDataReader reader = new ShapeDataReader(shapeFileName))
                {
                    var mbr = reader.ShapefileBounds;
                    var result = reader.ReadByMBRFilter(mbr);
                    var coll = result.GetEnumerator();

                    List<Street> streets = new List<Street>();

                    int i = 0;
                    while (coll.MoveNext())
                    {
                        ++i;

                        try
                        {
                            Street street = new Street();
                            street.NEWDIME_ID = coll.Current.Attributes["NEWDIME_ID"].ToString();

                            // There are a handful of records without a TAXKEY -- we'll just treat those as invalid, since they may have other weird data
                            if (string.IsNullOrEmpty(street.NEWDIME_ID))
                            {
                                _logger.LogTrace("Skipping record " + i.ToString() + " - Invalid NEWDIME_ID");
                                ++failure;
                                continue;
                            }

                            street.FNODE = int.Parse(coll.Current.Attributes["FNODE"].ToString());
                            street.TNODE = int.Parse(coll.Current.Attributes["TNODE"].ToString());
                            street.LPOLY = int.Parse(coll.Current.Attributes["LPOLY"].ToString());
                            street.RPOLY = int.Parse(coll.Current.Attributes["RPOLY"].ToString());
                            street.LENGTH = double.Parse(coll.Current.Attributes["LENGTH"].ToString());
                            street.NEWDIMENR = int.Parse(coll.Current.Attributes["NEWDIMENR"].ToString());
                            //street.NEWDIME_ID = int.Parse(coll.Current.Attributes["NEWDIME_ID"].ToString());
                            //street.MSLINK = int.Parse(coll.Current.Attributes["MSLINK"].ToString());
                            street.RCD_NBR = int.Parse(coll.Current.Attributes["RCD_NBR"].ToString());
                            street.TRANS_ID = int.Parse(coll.Current.Attributes["TRANS_ID"].ToString());
                            street.SQUAD_L = int.Parse(coll.Current.Attributes["SQUAD_L"].ToString());
                            street.BOILER_L = int.Parse(coll.Current.Attributes["BOILER_L"].ToString());
                            street.BI_CONST_L = int.Parse(coll.Current.Attributes["BI_CONST_L"].ToString());
                            street.BI_ELECT_L = int.Parse(coll.Current.Attributes["BI_ELECT_L"].ToString());
                            street.BI_ELEV_L = int.Parse(coll.Current.Attributes["BI_ELEV_L"].ToString());
                            street.BI_PLUMB_L = int.Parse(coll.Current.Attributes["BI_PLUMB_L"].ToString());
                            street.BI_SPRINK_ = int.Parse(coll.Current.Attributes["BI_SPRINK_"].ToString());
                            street.BI_CNDMN_L = int.Parse(coll.Current.Attributes["BI_CNDMN_L"].ToString());
                            street.CNTYNAME_L = coll.Current.Attributes["CNTYNAME_L"].ToString();
                            street.CNTY_L = int.Parse(coll.Current.Attributes["CNTY_L"].ToString());
                            street.MUNI_L = coll.Current.Attributes["MUNI_L"].ToString();
                            street.FMCD_L = int.Parse(coll.Current.Attributes["FMCD_L"].ToString());
                            street.FBLOCK_L = coll.Current.Attributes["FBLOCK_L"].ToString();
                            street.FTRACT_L = coll.Current.Attributes["FTRACT_L"].ToString();
                            street.ZIP_L = int.Parse(coll.Current.Attributes["ZIP_L"].ToString());
                            street.QTR_SECT_L = coll.Current.Attributes["QTR_SECT_L"].ToString();
                            street.WW_PRES_L = int.Parse(coll.Current.Attributes["WW_PRES_L"].ToString());
                            street.WW_SERV_L = int.Parse(coll.Current.Attributes["WW_SERV_L"].ToString());
                            street.MPS_ELEM_L = int.Parse(coll.Current.Attributes["MPS_ELEM_L"].ToString());
                            street.MPS_MS_L = int.Parse(coll.Current.Attributes["MPS_MS_L"].ToString());
                            street.MPS_HS_L = int.Parse(coll.Current.Attributes["MPS_HS_L"].ToString());
                            street.POLICE_L = int.Parse(coll.Current.Attributes["POLICE_L"].ToString());
                            street.ST_MAIN_L = coll.Current.Attributes["ST_MAIN_L"].ToString();
                            street.SAN_DIST_L = coll.Current.Attributes["SAN_DIST_L"].ToString();
                            street.FOR_TR_L = int.Parse(coll.Current.Attributes["FOR_TR_L"].ToString());
                            street.FOR_BL_L = int.Parse(coll.Current.Attributes["FOR_BL_L"].ToString());
                            street.SUM_RT_L = coll.Current.Attributes["SUM_RT_L"].ToString();
                            street.SUM_DA_L = coll.Current.Attributes["SUM_DA_L"].ToString();
                            street.WARD2K_L = int.Parse(coll.Current.Attributes["WARD2K_L"].ToString());
                            street.TRACT2K_L = int.Parse(coll.Current.Attributes["TRACT2K_L"].ToString());
                            street.BLOCK2K_L = coll.Current.Attributes["BLOCK2K_L"].ToString();
                            street.CONGR2K_L = int.Parse(coll.Current.Attributes["CONGR2K_L"].ToString());
                            street.STSEN2K_L = int.Parse(coll.Current.Attributes["STSEN2K_L"].ToString());
                            street.STASS2K_L = int.Parse(coll.Current.Attributes["STASS2K_L"].ToString());
                            street.CSUP2K_L = int.Parse(coll.Current.Attributes["CSUP2K_L"].ToString());
                            street.FIREBAT_L = int.Parse(coll.Current.Attributes["FIREBAT_L"].ToString());
                            street.SCHOOL2K_L = int.Parse(coll.Current.Attributes["SCHOOL2K_L"].ToString());
                            street.POLRD_L = int.Parse(coll.Current.Attributes["POLRD_L"].ToString());
                            street.ALD2004_L = int.Parse(coll.Current.Attributes["ALD2004_L"].ToString());
                            street.WIN_RT_L = coll.Current.Attributes["WIN_RT_L"].ToString();
                            street.RECYC_SM_L = coll.Current.Attributes["RECYC_SM_L"].ToString();
                            street.MUNICODE_L = coll.Current.Attributes["MUNICODE_L"].ToString();
                            street.WW_ROUT_L = int.Parse(coll.Current.Attributes["WW_ROUT_L"].ToString());
                            street.RECYC_DA_L = coll.Current.Attributes["RECYC_DA_L"].ToString();
                            street.RECYC_WN_L = coll.Current.Attributes["RECYC_WN_L"].ToString();
                            street.WTR16TH_L = coll.Current.Attributes["WTR16TH_L"].ToString();
                            street.SANLEAF_L = coll.Current.Attributes["SANLEAF_L"].ToString();
                            street.SANPLOW_L = coll.Current.Attributes["SANPLOW_L"].ToString();
                            street.BROOM_L = coll.Current.Attributes["BROOM_L"].ToString();
                            street.BROOMALL_L = coll.Current.Attributes["BROOMALL_L"].ToString();
                            street.LOCDIST_L = coll.Current.Attributes["LOCDIST_L"].ToString();
                            street.FOODINSP_L = int.Parse(coll.Current.Attributes["FOODINSP_L"].ToString());
                            street.CIPAREA_L = coll.Current.Attributes["CIPAREA_L"].ToString();
                            street.TRACT_L = int.Parse(coll.Current.Attributes["TRACT_L"].ToString());
                            street.ALD_L = int.Parse(coll.Current.Attributes["ALD_L"].ToString());
                            street.WARD_L = int.Parse(coll.Current.Attributes["WARD_L"].ToString());
                            street.SCHOOL_L = int.Parse(coll.Current.Attributes["SCHOOL_L"].ToString());
                            street.BLOCK_L = coll.Current.Attributes["BLOCK_L"].ToString();
                            street.STASS_L = int.Parse(coll.Current.Attributes["STASS_L"].ToString());
                            street.STSEN_L = int.Parse(coll.Current.Attributes["STSEN_L"].ToString());
                            street.CNTYSUP_L = int.Parse(coll.Current.Attributes["CNTYSUP_L"].ToString());
                            street.COMBSEW_L = coll.Current.Attributes["COMBSEW_L"].ToString();
                            street.SANBIZPL_L = coll.Current.Attributes["SANBIZPL_L"].ToString();
                            street.ST_OP_L = int.Parse(coll.Current.Attributes["ST_OP_L"].ToString());
                            street.FOR_PM_L = int.Parse(coll.Current.Attributes["FOR_PM_L"].ToString());
                            street.CONSERVE_L = coll.Current.Attributes["CONSERVE_L"].ToString();
                            street.SQUAD_R = int.Parse(coll.Current.Attributes["SQUAD_R"].ToString());
                            street.BOILER_R = int.Parse(coll.Current.Attributes["BOILER_R"].ToString());
                            street.BI_CONST_R = int.Parse(coll.Current.Attributes["BI_CONST_R"].ToString());
                            street.BI_ELECT_R = int.Parse(coll.Current.Attributes["BI_ELECT_R"].ToString());
                            street.BI_ELEV_R = int.Parse(coll.Current.Attributes["BI_ELEV_R"].ToString());
                            street.BI_PLUMB_R = int.Parse(coll.Current.Attributes["BI_PLUMB_R"].ToString());
                            street.SPRINK_R = int.Parse(coll.Current.Attributes["SPRINK_R"].ToString());
                            street.BI_CNDMN_R = int.Parse(coll.Current.Attributes["BI_CNDMN_R"].ToString());
                            street.CNTYNAME_R = coll.Current.Attributes["CNTYNAME_R"].ToString();
                            street.CNTY_R = int.Parse(coll.Current.Attributes["CNTY_R"].ToString());
                            street.MUNI_R = coll.Current.Attributes["MUNI_R"].ToString();
                            street.FMCD_R = int.Parse(coll.Current.Attributes["FMCD_R"].ToString());
                            street.FBLOCK_R = coll.Current.Attributes["FBLOCK_R"].ToString();
                            street.FTRACT_R = coll.Current.Attributes["FTRACT_R"].ToString();
                            street.ZIP_R = int.Parse(coll.Current.Attributes["ZIP_R"].ToString());
                            street.QTR_SECT_R = coll.Current.Attributes["QTR_SECT_R"].ToString();
                            street.WW_PRES_R = int.Parse(coll.Current.Attributes["WW_PRES_R"].ToString());
                            street.WW_SERV_R = int.Parse(coll.Current.Attributes["WW_SERV_R"].ToString());
                            street.MPS_ELEM_R = int.Parse(coll.Current.Attributes["MPS_ELEM_R"].ToString());
                            street.MPS_MS_R = int.Parse(coll.Current.Attributes["MPS_MS_R"].ToString());
                            street.MPS_HS_R = int.Parse(coll.Current.Attributes["MPS_HS_R"].ToString());
                            street.POLICE_R = int.Parse(coll.Current.Attributes["POLICE_R"].ToString());
                            street.ST_MAIN_R = coll.Current.Attributes["ST_MAIN_R"].ToString();
                            street.SAN_DIST_R = coll.Current.Attributes["SAN_DIST_R"].ToString();
                            street.FOR_TR_R = int.Parse(coll.Current.Attributes["FOR_TR_R"].ToString());
                            street.FOR_BL_R = int.Parse(coll.Current.Attributes["FOR_BL_R"].ToString());
                            street.SUM_RT_R = coll.Current.Attributes["SUM_RT_R"].ToString();
                            street.SUM_DA_R = coll.Current.Attributes["SUM_DA_R"].ToString();
                            street.WARD2K_R = int.Parse(coll.Current.Attributes["WARD2K_R"].ToString());
                            street.TRACT2K_R = int.Parse(coll.Current.Attributes["TRACT2K_R"].ToString());
                            street.BLOCK2K_R = coll.Current.Attributes["BLOCK2K_R"].ToString();
                            street.CONGR2K_R = int.Parse(coll.Current.Attributes["CONGR2K_R"].ToString());
                            street.STSEN2K_R = int.Parse(coll.Current.Attributes["STSEN2K_R"].ToString());
                            street.STASS2K_R = int.Parse(coll.Current.Attributes["STASS2K_R"].ToString());
                            street.CSUP2K_R = int.Parse(coll.Current.Attributes["CSUP2K_R"].ToString());
                            street.FIREBAT_R = int.Parse(coll.Current.Attributes["FIREBAT_R"].ToString());
                            street.SCHOOL2K_R = int.Parse(coll.Current.Attributes["SCHOOL2K_R"].ToString());
                            street.POLRD_R = int.Parse(coll.Current.Attributes["POLRD_R"].ToString());
                            street.ALD2004_R = int.Parse(coll.Current.Attributes["ALD2004_R"].ToString());
                            street.WIN_RT_R = coll.Current.Attributes["WIN_RT_R"].ToString();
                            street.RECYC_SM_R = coll.Current.Attributes["RECYC_SM_R"].ToString();
                            street.MUNICODE_R = coll.Current.Attributes["MUNICODE_R"].ToString();
                            street.WW_ROUT_R = int.Parse(coll.Current.Attributes["WW_ROUT_R"].ToString());
                            street.RECYC_DA_R = coll.Current.Attributes["RECYC_DA_R"].ToString();
                            street.RECYC_WN_R = coll.Current.Attributes["RECYC_WN_R"].ToString();
                            street.WTR16TH_R = coll.Current.Attributes["WTR16TH_R"].ToString();
                            street.SANLEAF_R = coll.Current.Attributes["SANLEAF_R"].ToString();
                            street.SANPLOW_R = coll.Current.Attributes["SANPLOW_R"].ToString();
                            street.BROOM_R = coll.Current.Attributes["BROOM_R"].ToString();
                            street.BROOMALL_R = coll.Current.Attributes["BROOMALL_R"].ToString();
                            street.LOCDIST_R = coll.Current.Attributes["LOCDIST_R"].ToString();
                            street.FOODINSP_R = int.Parse(coll.Current.Attributes["FOODINSP_R"].ToString());
                            street.CIPAREA_R = coll.Current.Attributes["CIPAREA_R"].ToString();
                            street.TRACT_R = int.Parse(coll.Current.Attributes["TRACT_R"].ToString());
                            street.ALD_R = int.Parse(coll.Current.Attributes["ALD_R"].ToString());
                            street.WARD_R = int.Parse(coll.Current.Attributes["WARD_R"].ToString());
                            street.SCHOOL_R = int.Parse(coll.Current.Attributes["SCHOOL_R"].ToString());
                            street.BLOCK_R = coll.Current.Attributes["BLOCK_R"].ToString();
                            street.STASS_R = int.Parse(coll.Current.Attributes["STASS_R"].ToString());
                            street.STSEN_R = int.Parse(coll.Current.Attributes["STSEN_R"].ToString());
                            street.CNTYSUP_R = int.Parse(coll.Current.Attributes["CNTYSUP_R"].ToString());
                            street.COMBSEW_R = coll.Current.Attributes["COMBSEW_R"].ToString();
                            street.SANBIZPL_R = coll.Current.Attributes["SANBIZPL_R"].ToString();
                            street.ST_OP_R = int.Parse(coll.Current.Attributes["ST_OP_R"].ToString());
                            street.FOR_PM_R = int.Parse(coll.Current.Attributes["FOR_PM_R"].ToString());
                            street.CONSERVE_R = coll.Current.Attributes["CONSERVE_R"].ToString();
                            street.SEG_L_TYPE = coll.Current.Attributes["SEG_L_TYPE"].ToString();
                            street.LEVEL = int.Parse(coll.Current.Attributes["LEVEL"].ToString());
                            street.DIR = coll.Current.Attributes["DIR"].ToString();
                            street.STREET = coll.Current.Attributes["STREET"].ToString();
                            street.STTYPE = coll.Current.Attributes["STTYPE"].ToString();
                            street.LO_ADD_L = int.Parse(coll.Current.Attributes["LO_ADD_L"].ToString());
                            street.HI_ADD_L = int.Parse(coll.Current.Attributes["HI_ADD_L"].ToString());
                            street.LO_ADD_R = int.Parse(coll.Current.Attributes["LO_ADD_R"].ToString());
                            street.HI_ADD_R = int.Parse(coll.Current.Attributes["HI_ADD_R"].ToString());
                            street.BUS_L = coll.Current.Attributes["BUS_L"].ToString();
                            street.BUS_R = coll.Current.Attributes["BUS_R"].ToString();
                            street.STCLASS = int.Parse(coll.Current.Attributes["STCLASS"].ToString());
                            street.CFCC = coll.Current.Attributes["CFCC"].ToString();
                            street.FROM_NODE = int.Parse(coll.Current.Attributes["FROM_NODE"].ToString());
                            street.TO_NODE = int.Parse(coll.Current.Attributes["TO_NODE"].ToString());
                            street.LOW_X = double.Parse(coll.Current.Attributes["LOW_X"].ToString());
                            street.LOW_Y = double.Parse(coll.Current.Attributes["LOW_Y"].ToString());
                            street.HIGH_X = double.Parse(coll.Current.Attributes["HIGH_X"].ToString());
                            street.HIGH_Y = double.Parse(coll.Current.Attributes["HIGH_Y"].ToString());

                            GeometryFactory geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

                            //IPoint projectedCentroid = GeographicUtilities.ReprojectCoordinates(projectionInfo, coll.Current.Geometry.Centroid);
                            //IPoint transformedCentroid = geometryFactory.CreatePoint(new Coordinate(projectedCentroid.X, projectedCentroid.Y));
                            //street.Centroid = transformedCentroid;

                            Coordinate[] projectedCoordinates = GeographicUtilities.ReprojectCoordinates(projectionInfo, coll.Current.Geometry.Coordinates);
                            //Polygon transformedGeometry = (Polygon)geometryFactory.CreatePolygon(projectedCoordinates);
                            LineString transformedGeometry = (LineString)geometryFactory.CreateLineString(projectedCoordinates);

                            // https://gis.stackexchange.com/questions/289545/using-sqlgeometry-makevalid-to-get-a-counter-clockwise-polygon-in-sql-server
                            //if (!transformedGeometry.Shell.IsCCW)
                            //    transformedGeometry = (Polygon)transformedGeometry.Reverse();

                            street.Outline = transformedGeometry;
                            GeographicUtilities.SetBounds(street, street.Outline);

                            streets.Add(street);

                            if (i % 100 == 0)
                            {
                                try
                                {
                                    Tuple<IEnumerable<Street>, IEnumerable<Street>> results1 = await _streetWriteService.BulkCreate(claimsPrincipal, streets, false);
                                    success += results1.Item1.Count();
                                    failure += results1.Item2.Count();
                                    streets.Clear();

                                    _logger.LogDebug("Bulk inserted items at mod " + i.ToString());
                                }
                                catch (Exception ex)
                                {
                                    failure += streets.Count;

                                    _logger.LogError(ex, "Error bulk inserting items at mod " + i.ToString());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error importing Street");
                        }
                    }

                    try
                    {
                        Tuple<IEnumerable<Street>, IEnumerable<Street>> results2 = await _streetWriteService.BulkCreate(claimsPrincipal, streets, false);
                        success += results2.Item1.Count();
                        failure += results2.Item2.Count();

                        _logger.LogDebug("Bulk inserted items at mod " + i.ToString());
                    }
                    catch (Exception ex)
                    {
                        failure += streets.Count;

                        _logger.LogError(ex, "Error bulk inserting items at mod " + i.ToString());
                    }
                }

                _logger.LogInformation("Import results: " + success.ToString() + " success, " + failure.ToString() + " failure");

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
