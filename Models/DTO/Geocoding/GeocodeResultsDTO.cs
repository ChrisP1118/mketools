﻿using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.DTO.Geocoding
{
    public class GeocodeResultsDTO
    {
        [JsonConverter(typeof(GeometryConverter))]
        public Geometry Geometry { get; set; }

        public GeometryAccuracy Accuracy { get; set; }
        public GeometrySource Source { get; set; }
    }
}
