﻿using GeoAPI.Geometries;
using MkeAlerts.Web.Models.Data;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Subscriptions
{
    public class DispatchCallSubscriptionDTO
    {
        public Guid Id { get; set; }

        public Guid ApplicationUserId { get; set; }

        public DispatchCallType DispatchCallType { get; set; }

        [JsonConverter(typeof(GeometryConverter))]
        public IGeometry Point { get; set; }
        public int Distance { get; set; }

        public string SDIR { get; set; }
        public string STREET { get; set; }
        public string STTYPE { get; set; }
        public int HOUSE_NR { get; set; }
    }
}