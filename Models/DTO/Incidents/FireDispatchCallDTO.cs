using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Incidents
{
    public class FireDispatchCallDTO
    {
        public string CFS { get; set; }

        [JsonConverter(typeof(GeometryConverter))]
        public Geometry Geometry { get; set; }

        public DateTime ReportedDateTime { get; set; }
        public string Address { get; set; }
        public string Apt { get; set; }
        public string City { get; set; }
        public string NatureOfCall { get; set; }
        public string Disposition { get; set; }
    }
}
