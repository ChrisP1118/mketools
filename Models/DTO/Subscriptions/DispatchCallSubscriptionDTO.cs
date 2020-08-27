using MkeTools.Web.Models.Data;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.DTO.Subscriptions
{
    public class DispatchCallSubscriptionDTO
    {
        public Guid Id { get; set; }

        public Guid ApplicationUserId { get; set; }

        public DispatchCallType DispatchCallType { get; set; }

        [JsonConverter(typeof(GeometryConverter))]
        public Geometry Point { get; set; }
        public int Distance { get; set; }

        public string SDIR { get; set; }
        public string STREET { get; set; }
        public string STTYPE { get; set; }
        public int HOUSE_NR { get; set; }
    }
}
