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

        public ImportParcelsJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<ImportParcelsJob> logger, IEntityWriteService<Parcel, string> parcelWriteService)
            : base(configuration, signInManager, userManager)
        {
            _logger = logger;
            _parcelWriteService = parcelWriteService;
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task Run()
        {
            _logger.LogInformation("Starting job");

            string fileName = await PackageUtilities.DownloadPackageFile(_logger, "parcel-outlines", null, "parcelbase");

            _logger.LogDebug("Download complete: " + fileName);

            string folderName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(folderName);

            ZipFile.ExtractToDirectory(fileName, folderName);
            _logger.LogDebug(fileName + " unzipped to " + folderName);

            string shapeFileName = folderName + "\\parcelbase_mprop_full.shp";
            string projectionFileName = folderName + "\\parcelbase_mprop_full.prj";

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

                int i = 0;
                while (coll.MoveNext())
                {
                    ++i;

                    try
                    {
                        Parcel parcel = new Parcel();
                        parcel.TAXKEY = coll.Current.Attributes["TAXKEY"].ToString();

                        // There are a handful of records without a TAXKEY -- we'll just treat those as invalid, since they may have other weird data
                        if (string.IsNullOrEmpty(parcel.TAXKEY) || parcel.TAXKEY == "<Null>")
                        {
                            _logger.LogTrace("Skipping record " + i.ToString() + " - Invalid TAXKEY");
                            ++failure;
                            continue;
                        }

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

                        parcel.Outline = transformedGeometry;
                        GeographicUtilities.SetBounds(parcel, parcel.Outline);

                        parcels.Add(parcel);

                        if (i % 100 == 0)
                        {
                            try
                            {
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

            foreach (string file in Directory.EnumerateFiles(folderName))
                File.Delete(file);
            Directory.Delete(folderName);

            _logger.LogInformation("Finishing job");
        }
    }
}
