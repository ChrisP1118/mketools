using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.DTO.Places
{
    public class AddressDTO
    {
        public ParcelDTO Parcel { get; set; }

        public int HouseNumber { get; set; }

        public int ADDRESS_ID { get; set; }

        [JsonConverter(typeof(GeometryConverter))]
        public Point Point { get; set; }

        public double MinLat { get; set; }
        public double MaxLat { get; set; }
        public double MinLng { get; set; }
        public double MaxLng { get; set; }

        public int OBJECTID { get; set; }

        [MaxLength(10)] public string TAXKEY { get; set; }
        [MaxLength(15)] public string HOUSENO { get; set; }
        [MaxLength(2)] public string HOUSESX { get; set; }
        [MaxLength(15)] public string ALT_ID { get; set; }
        [MaxLength(1)] public string DIR { get; set; }
        [MaxLength(19)] public string STREET { get; set; }
        [MaxLength(4)] public string STTYPE { get; set; }
        [MaxLength(1)] public string PDIR { get; set; }
        [MaxLength(15)] public string MUNI { get; set; }
        [MaxLength(5)] public string UNIT { get; set; }
        [MaxLength(9)] public string ZIP_CODE { get; set; }
        //public DateTime DATE_CHANG { get; set; }
        [MaxLength(140)] public string COMMENT { get; set; }
        [MaxLength(36)] public string SOURCE { get; set; }
        //public DateTime SOURCE_DAT { get; set; }
        [MaxLength(21)] public string SOURCE_ID { get; set; }
        public int MAILABLE { get; set; }
        [MaxLength(30)] public string FULLADDR { get; set; }
        [MaxLength(1)] public string X { get; set; }
        [MaxLength(1)] public string Y { get; set; }
        [MaxLength(1)] public string DD_LAT { get; set; }
        [MaxLength(1)] public string DD_LONG { get; set; }
        [MaxLength(41)] public string TAG { get; set; }
        [MaxLength(1)] public string CLINEID { get; set; }
        public int BUILDING_I { get; set; }
    }
}
