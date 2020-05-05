using DotSpatial.Projections;
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

        public static void SetBounds(IHasBounds hasBounds, Geometry geometry)
        {
            if (geometry?.Coordinates == null)
                return;

            if (geometry.Coordinates.Length == 0)
                return;

            // Two digits after the decimal
            double adjustment = Math.Pow(10, 10);
            hasBounds.MinLat = Math.Floor(geometry.Coordinates.Select(x => x.Y).Min() * adjustment) / adjustment;
            hasBounds.MaxLat = Math.Ceiling(geometry.Coordinates.Select(x => x.Y).Max() * adjustment) / adjustment;
            hasBounds.MinLng = Math.Floor(geometry.Coordinates.Select(x => x.X).Min() * adjustment) / adjustment;
            hasBounds.MaxLng = Math.Ceiling(geometry.Coordinates.Select(x => x.X).Max() * adjustment) / adjustment;
        }
    }
}
