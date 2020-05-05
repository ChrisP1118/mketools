using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Places
{
    public class StreetDTO
    {
        [JsonConverter(typeof(GeometryConverter))]
        public Geometry Outline { get; set; }

        public double MinLat { get; set; }
        public double MaxLat { get; set; }
        public double MinLng { get; set; }
        public double MaxLng { get; set; }

        public int OBJECTID { get; set; }

        public int CLINEID { get; set; }

        [MaxLength(2)] public string DIR { get; set; }
        [MaxLength(24)] public string STREET { get; set; }
        [MaxLength(4)] public string STTYPE { get; set; }
        [MaxLength(1)] public string PDIR { get; set; }
        [MaxLength(5)] public string LOW { get; set; }
        [MaxLength(5)] public string HIGH { get; set; }
        [MaxLength(5)] public string LEFTFR { get; set; }
        [MaxLength(5)] public string LEFTTO { get; set; }
        [MaxLength(5)] public string RIGHTFR { get; set; }
        [MaxLength(5)] public string RIGHTTO { get; set; }
        [MaxLength(15)] public string MUNI { get; set; }
        [MaxLength(3)] public string FCC { get; set; }
        [MaxLength(1)] public string OWNER { get; set; }
        [MaxLength(5)] public string R_MCD { get; set; }
        [MaxLength(5)] public string L_MCD { get; set; }
        [MaxLength(1)] public string SHIELD { get; set; }
        [MaxLength(16)] public string HIGHWAY { get; set; }
        [MaxLength(1)] public string SOURCE { get; set; }
        [MaxLength(1)] public string COMMENT { get; set; }
        [MaxLength(1)] public string DATE_CHANG { get; set; }

    }
}
