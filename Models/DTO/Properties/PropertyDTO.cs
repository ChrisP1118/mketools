using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Properties
{
    public class PropertyDTO
    {
        [MaxLength(10)]
        public string TAXKEY { get; set; }

        [MaxLength(3)] public string AIR_CONDIT { get; set; }
        public float ANGLE { get; set; }
        [MaxLength(1)] public string ATTIC { get; set; }
        [MaxLength(1)] public string BASEMENT { get; set; }
        public int BATHS { get; set; }
        public int BEDROOMS { get; set; }
        public int BLDG_AREA { get; set; }
        [MaxLength(9)] public string BLDG_TYPE { get; set; }
        [MaxLength(1)] public string C_A_CLASS { get; set; }
        public int C_A_EXM_IM { get; set; }
        public int C_A_EXM_LA { get; set; }
        public int C_A_EXM_TO { get; set; }
        [MaxLength(3)] public string C_A_EXM_TY { get; set; }
        public int C_A_IMPRV { get; set; }
        public int C_A_LAND { get; set; }
        [MaxLength(1)] public string C_A_SYMBOL { get; set; }
        public int C_A_TOTAL { get; set; }
        [MaxLength(6)] public string CHG_NR { get; set; }
        [MaxLength(1)] public string CHK_DIGIT { get; set; }
        public DateTime? CONVEY_DAT { get; set; }
        public float CONVEY_FEE { get; set; }
        [MaxLength(2)] public string CONVEY_TYP { get; set; }
        [MaxLength(1)] public string CORNER_LOT { get; set; }
        [MaxLength(1)] public string SDIR { get; set; }
        public int DIV_DROP { get; set; }
        public int DIV_ORG { get; set; }
        [MaxLength(2)] public string DPW_SANITA { get; set; }
        public float EXM_ACREAG { get; set; }
        public float EXM_PER__1 { get; set; }
        public float EXM_PER_CT { get; set; }
        [MaxLength(1)] public string FIREPLACE { get; set; }
        [MaxLength(2)] public string GEO_ALDER { get; set; }
        [MaxLength(2)] public string GEO_ALDER_ { get; set; }
        [MaxLength(2)] public string GEO_BI_MAI { get; set; }
        [MaxLength(4)] public string GEO_BLOCK { get; set; }
        [MaxLength(2)] public string GEO_FIRE { get; set; }
        [MaxLength(2)] public string GEO_POLICE { get; set; }
        [MaxLength(6)] public string GEO_TRACT { get; set; }
        [MaxLength(9)] public string GEO_ZIP_CO { get; set; }
        [MaxLength(1)] public string HIST_CODE { get; set; }
        public int HOUSE_NR_H { get; set; }
        public int HOUSE_NR_L { get; set; }
        [MaxLength(3)] public string HOUSE_NR_S { get; set; }
        [MaxLength(4)] public string LAND_USE { get; set; }
        [MaxLength(2)] public string LAND_USE_G { get; set; }
        public DateTime? LAST_NAME_ { get; set; }
        public DateTime? LAST_VALUE { get; set; }
        public int LOT_AREA { get; set; }
        [MaxLength(4)] public string NEIGHBORHO { get; set; }
        [MaxLength(4)] public string NR_ROOMS { get; set; }
        public float NR_STORIES { get; set; }
        public int NR_UNITS { get; set; }
        [MaxLength(23)] public string OWNER_CITY { get; set; }
        [MaxLength(28)] public string OWNER_MAIL { get; set; }
        [MaxLength(28)] public string OWNER_NA_1 { get; set; }
        [MaxLength(28)] public string OWNER_NA_2 { get; set; }
        [MaxLength(28)] public string OWNER_NAME { get; set; }
        [MaxLength(9)] public string OWNER_ZIP { get; set; }
        [MaxLength(1)] public string OWN_OCPD { get; set; }
        [MaxLength(1)] public string P_A_CLASS { get; set; }
        public int P_A_EXM_IM { get; set; }
        public int P_A_EXM_LA { get; set; }
        public int P_A_EXM_TO { get; set; }
        [MaxLength(3)] public string P_A_EXM_TY { get; set; }
        public int P_A_IMPRV { get; set; }
        public int P_A_LAND { get; set; }
        [MaxLength(1)] public string P_A_SYMBOL { get; set; }
        public int P_A_TOTAL { get; set; }
        public float PARCEL_TYP { get; set; }
        public float PARKING_SP { get; set; }
        [MaxLength(2)] public string PARKING_TY { get; set; }
        [MaxLength(5)] public string PLAT_PAGE { get; set; }
        public int POWDER_ROO { get; set; }
        [MaxLength(1)] public string RAZE_STATU { get; set; }
        [MaxLength(3)] public string REASON_FOR { get; set; }
        [MaxLength(18)] public string STREET { get; set; }
        [MaxLength(2)] public string STTYPE { get; set; }
        [MaxLength(1)] public string SUB_ACCT { get; set; }
        [MaxLength(1)] public string SWIM_POOL { get; set; }
        [MaxLength(2)] public string TAX_RATE_C { get; set; }
        [MaxLength(4)] public string BI_VIOL { get; set; }
        public int TAX_DELQ { get; set; }
        [MaxLength(4)] public string YR_ASSMT { get; set; }
        [MaxLength(4)] public string YR_BUILT { get; set; }
        [MaxLength(7)] public string ZONING { get; set; }
    }
}
