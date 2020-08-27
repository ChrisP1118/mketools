using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.DTO.Places
{
    public class CommonParcelDTO
    {
        public int MAP_ID { get; set; }

        [JsonConverter(typeof(GeometryConverter))]
        public Geometry Outline { get; set; }

        public double MinLat { get; set; }
        public double MaxLat { get; set; }
        public double MinLng { get; set; }
        public double MaxLng { get; set; }

        public List<ParcelDTO> Parcels { get; set; }

    }
}
