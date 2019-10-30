using GeoAPI.Geometries;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Places
{
    public class CommonParcelDTO
    {
        public Guid Id { get; set; }

        [JsonConverter(typeof(GeometryConverter))]
        public IGeometry Outline { get; set; }

        public List<ParcelDTO> Parcels { get; set; }
    }
}
