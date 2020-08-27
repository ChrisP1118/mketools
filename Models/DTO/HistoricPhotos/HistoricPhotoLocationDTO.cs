using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.DTO.HistoricPhotos
{
    public class HistoricPhotoLocationDTO
    {
        public Guid Id { get; set; }

        [JsonConverter(typeof(GeometryConverter))]
        public Geometry Geometry { get; set; }

        public List<HistoricPhotoDTO> HistoricPhotos { get; set; }
    }
}
