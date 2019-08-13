using DotSpatial.Projections;
using GeoAPI.Geometries;
using MkeAlerts.Web.Models.Data;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Utilities
{
    public static class GeographicUtilities
    {
        public static Tuple<double, double> ReprojectCoordinates(ProjectionInfo source, double x, double y)
        {
            ProjectionInfo target = KnownCoordinateSystems.Geographic.World.ITRF2000;

            // This is what Google, Bing, etc. use for mapping - but projecting to it seems to end up with coordinates that are off by a few hundred feet
            //ProjectionInfo target = KnownCoordinateSystems.Geographic.World.WGS1984;

            double[] xy = new double[2]
            {
                x,
                y
            };
            double[] z = new double[1] { 1 };
            Reproject.ReprojectPoints(xy, z, source, target, 0, 1);

            return new Tuple<double, double>(xy[0], xy[1]);
        }

        public static IPoint ReprojectCoordinates(ProjectionInfo source, IPoint point)
        {
            Tuple<double, double> coordinates = ReprojectCoordinates(source, point.X, point.Y);
            return new Point(coordinates.Item1, coordinates.Item2);
        }

        public static Coordinate[] ReprojectCoordinates(ProjectionInfo source, Coordinate[] coordinates)
        {
            Coordinate[] retVal = new Coordinate[coordinates.Length];
            for (int i = 0; i < coordinates.Length; ++i)
            {
                Tuple<double, double> coordinateSet = ReprojectCoordinates(source, coordinates[i].X, coordinates[i].Y);
                retVal[i] = new Coordinate(coordinateSet.Item1, coordinateSet.Item2);
            }
            return retVal;
        }

        public static void SetBounds(IHasBounds hasBounds, IGeometry geometry)
        {
            // Two digits after the decimal
            double adjustment = Math.Pow(10, 2);
            hasBounds.MinLat = Math.Floor(geometry.Coordinates.Select(x => x.Y).Min() * adjustment) / adjustment;
            hasBounds.MaxLat = Math.Ceiling(geometry.Coordinates.Select(x => x.Y).Max() * adjustment) / adjustment;
            hasBounds.MinLng = Math.Floor(geometry.Coordinates.Select(x => x.X).Min() * adjustment) / adjustment;
            hasBounds.MaxLng = Math.Ceiling(geometry.Coordinates.Select(x => x.X).Max() * adjustment) / adjustment;
        }
    }
}
