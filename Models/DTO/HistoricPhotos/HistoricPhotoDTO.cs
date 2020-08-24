using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.HistoricPhotos
{
    public class HistoricPhotoDTO
    {
        public string Id { get; set; }

        [JsonConverter(typeof(GeometryConverter))]
        public Geometry Geometry { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public int? Year { get; set; }
        public string Place { get; set; }
        public string CurrentAddress { get; set; }
        public string OldAddress { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
    }
}
