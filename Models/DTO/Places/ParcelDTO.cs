using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Places
{
    public class ParcelDTO
    {
        public CommonParcelDTO CommonParcel { get; set; }

        public List<AddressDTO> Addresses { get; set; }
        public List<PropertyDTO> Properties { get; set; }
        public CurrentPropertyDTO CurrentProperty { get; set; }

        public int HouseNumber { get; set; }
        public int HouseNumberHigh { get; set; }

        public int OBJECTID { get; set; }
        public int MAP_ID { get; set; }
        [MaxLength(10)] public string TAXKEY { get; set; }
        [MaxLength(50)] public string PARCEL_KEY { get; set; }
        [MaxLength(30)] public string PARCEL_DES { get; set; }
        //public DateTime RECORD_DAT { get; set; }
        public int OVERLAP { get; set; }
        [MaxLength(5)] public string MCD { get; set; }
        [MaxLength(39)] public string SOURCE { get; set; }
        [MaxLength(84)] public string COMMENT { get; set; }
        public int RECID { get; set; }
        [MaxLength(7)] public string RECSOURCE { get; set; }
        //public DateTime RECDATE { get; set; }
        [MaxLength(15)] public string MUNINAME { get; set; }
        [MaxLength(10)] public string PARCELNO { get; set; }
        [MaxLength(84)] public string OWNERNAME1 { get; set; }
        [MaxLength(84)] public string OWNERNAME2 { get; set; }
        [MaxLength(28)] public string OWNERNAME3 { get; set; }
        [MaxLength(49)] public string OWNERADDR { get; set; }
        [MaxLength(27)] public string OWNERCTYST { get; set; }
        [MaxLength(10)] public string OWNERZIP { get; set; }
        [MaxLength(10)] public string HOUSENR { get; set; }
        [MaxLength(10)] public string HOUSENRHI { get; set; }
        [MaxLength(3)] public string HOUSENRSFX { get; set; }
        [MaxLength(2)] public string STREETDIR { get; set; }
        [MaxLength(20)] public string STREETNAME { get; set; }
        [MaxLength(4)] public string STREETTYPE { get; set; }
        [MaxLength(2)] public string SUFFIXDIR { get; set; }
        [MaxLength(8)] public string UNITNUMBER { get; set; }
        [MaxLength(15)] public string POSTOFFICE { get; set; }
        [MaxLength(254)] public string LEGALDESCR { get; set; }
        [MaxLength(74)] public string CONDO_NAME { get; set; }
        [MaxLength(7)] public string UNIT_TYPE { get; set; }
        public double ACRES { get; set; }
        public int ASSESSEDVA { get; set; }
        public int LANDVALUE { get; set; }
        public int IMPVALUE { get; set; }
        [MaxLength(1)] public string CLASS { get; set; }
        [MaxLength(2)] public string CODE { get; set; }
        [MaxLength(23)] public string DESCRIPTIO { get; set; }
        [MaxLength(1)] public string ZONING_COD { get; set; }
        [MaxLength(29)] public string ZONING_DES { get; set; }
        [MaxLength(88)] public string ZONING_URL { get; set; }
        [MaxLength(3)] public string EXM_TYP { get; set; }
        [MaxLength(94)] public string EXM_TYP_DE { get; set; }
        [MaxLength(76)] public string EXM_CLASS_ { get; set; }
        [MaxLength(1)] public string TAX_INFO_U { get; set; }
        [MaxLength(88)] public string ASSESSMENT { get; set; }
        [MaxLength(13)] public string PARCEL_TYP { get; set; }
        public int TAX_YR { get; set; }
        public int FAIR_MKT_V { get; set; }
        public double GROSS_TAX { get; set; }
        public double NET_TAX { get; set; }
        public double GIS_ACRES { get; set; }
        [MaxLength(41)] public string SCHOOL_DIS { get; set; }
        [MaxLength(4)] public string SCHOOL_ID { get; set; }
        [MaxLength(5)] public string PAR_ZIP { get; set; }
        [MaxLength(4)] public string PAR_ZIP_EX { get; set; }
        [MaxLength(32)] public string ADDRESS { get; set; }
        [MaxLength(8)] public string DWELLING_C { get; set; }
    }
}
