using GeoAPI.Geometries;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Geocoding
{
    public class GeocodeResultsDTO
    {
        [JsonConverter(typeof(GeometryConverter))]
        public IGeometry Geometry { get; set; }

        public GeometryAccuracy Accuracy { get; set; }
        public GeometrySource Source { get; set; }
    }
}
