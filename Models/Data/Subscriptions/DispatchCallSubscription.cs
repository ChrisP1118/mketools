﻿using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data.Subscriptions
{
    public class DispatchCallSubscription : Subscription
    {
        public DispatchCallType DispatchCallType { get; set; }

        public Point Point { get; set; }
        public int Distance { get; set; }

        [MaxLength(1)]
        public string SDIR { get; set; }

        [MaxLength(19)]
        public string STREET { get; set; }

        [MaxLength(4)]
        public string STTYPE { get; set; }

        public int HOUSE_NR { get; set; }
    }
}
