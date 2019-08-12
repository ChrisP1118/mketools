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
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Location = MkeAlerts.Web.Models.Data.Places.Location;

namespace MkeAlerts.Web.Jobs
{
    public class ImportLocationsJob : ImportJob
    {
        private readonly ILogger<ImportLocationsJob> _logger;
        private readonly IEntityWriteService<Location, string> _locationWriteService;

        public ImportLocationsJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<ImportLocationsJob> logger, IEntityWriteService<Location, string> locationWriteService)
            : base(configuration, signInManager, userManager)
        {
            _logger = logger;
            _locationWriteService = locationWriteService;
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task Run()
        {
            _logger.LogInformation("Starting job");

            ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

            string path = @"M:\My Documents\GitHub\mkealerts\DataSources\parcelbase_mprop_full\parcelbase_mprop_full.shp";

            var projectionInfo = ProjectionInfo.Open(path.Replace(".shp", ".prj"));

            int success = 0;
            int failure = 0;

            using (ShapeDataReader reader = new ShapeDataReader(path))
            {
                var mbr = reader.ShapefileBounds;
                var result = reader.ReadByMBRFilter(mbr);
                var coll = result.GetEnumerator();

                List<Location> locations = new List<Location>();

                int i = 0;
                while (coll.MoveNext())
                {
                    ++i;

                    try
                    {
                        Location location = new Location();
                        location.TAXKEY = coll.Current.Attributes["TAXKEY"].ToString();

                        // There are a handful of records without a TAXKEY -- we'll just treat those as invalid, since they may have other weird data
                        if (string.IsNullOrEmpty(location.TAXKEY) || location.TAXKEY == "<Null>")
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

                        location.Outline = transformedGeometry;

                        // Two digits after the decimal
                        double adjustment = Math.Pow(10, 2);
                        location.MinLat = Math.Floor(transformedGeometry.Coordinates.Select(x => x.Y).Min() * adjustment) / adjustment;
                        location.MaxLat = Math.Ceiling(transformedGeometry.Coordinates.Select(x => x.Y).Max() * adjustment) / adjustment;
                        location.MinLng = Math.Floor(transformedGeometry.Coordinates.Select(x => x.X).Min() * adjustment) / adjustment;
                        location.MaxLng = Math.Ceiling(transformedGeometry.Coordinates.Select(x => x.X).Max() * adjustment) / adjustment;

                        locations.Add(location);

                        if (i % 100 == 0)
                        {
                            Tuple<IEnumerable<Location>, IEnumerable<Location>> results1 = await _locationWriteService.BulkCreate(claimsPrincipal, locations, false);
                            success += results1.Item1.Count();
                            failure += results1.Item2.Count();
                            locations.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error importing Location");
                    }
                }

                Tuple<IEnumerable<Location>, IEnumerable<Location>> results2 = await _locationWriteService.BulkCreate(claimsPrincipal, locations, false);
                success += results2.Item1.Count();
                failure += results2.Item2.Count();
            }

            _logger.LogInformation("Import results: " + success.ToString() + " success, " + failure.ToString() + " failure");
            _logger.LogInformation("Finishing job");
        }
    }
}
