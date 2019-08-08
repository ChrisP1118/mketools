using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Internal
{
    public class GeocodeResults
    {
        public IGeometry Geometry { get; set; }

        public GeometryAccuracy Accuracy { get; set; }
        public GeometrySource Source { get; set; }
    }
}
