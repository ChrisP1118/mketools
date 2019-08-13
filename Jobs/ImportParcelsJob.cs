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
using Parcel = MkeAlerts.Web.Models.Data.Places.Parcel;

namespace MkeAlerts.Web.Jobs
{
    public class ImportParcelsJob : ImportJob
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
                            Tuple<IEnumerable<Parcel>, IEnumerable<Parcel>> results1 = await _parcelWriteService.BulkCreate(claimsPrincipal, parcels, false);
                            success += results1.Item1.Count();
                            failure += results1.Item2.Count();
                            parcels.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error importing Location");
                    }
                }

                Tuple<IEnumerable<Parcel>, IEnumerable<Parcel>> results2 = await _parcelWriteService.BulkCreate(claimsPrincipal, parcels, false);
                success += results2.Item1.Count();
                failure += results2.Item2.Count();
            }

            _logger.LogInformation("Import results: " + success.ToString() + " success, " + failure.ToString() + " failure");
            _logger.LogInformation("Finishing job");
        }
    }
}
