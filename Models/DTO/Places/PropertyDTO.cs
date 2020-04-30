using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Places
{
    public class PropertyDTO
    {
        public string TAXKEY { get; set; }

        //public List<Address> Addresses { get; set; }
        public ParcelDTO Parcel { get; set; }

        [MaxLength(3)] public string AIR_CONDITIONING { get; set; }
        [MaxLength(1)] public string ATTIC { get; set; }
        [MaxLength(1)] public string BASEMENT { get; set; }
        public int BATHS { get; set; }
        public int BEDROOMS { get; set; }
        public int BLDG_AREA { get; set; }
        [MaxLength(9)] public string BLDG_TYPE { get; set; }
        [MaxLength(1)] public string C_A_CLASS { get; set; }
        public int C_A_EXM_IMPRV { get; set; }
        public int C_A_EXM_LAND { get; set; }
        public int C_A_EXM_TOTAL { get; set; }
        [MaxLength(3)] public string C_A_EXM_TYPE { get; set; }
        public int C_A_IMPRV { get; set; }
        public int C_A_LAND { get; set; }
        [MaxLength(1)] public string C_A_SYMBOL { get; set; }
        public int C_A_TOTAL { get; set; }
        [MaxLength(6)] public string CHG_NR { get; set; }
        [MaxLength(1)] public string CHK_DIGIT { get; set; }
        public DateTime CONVEY_DATE { get; set; }
        public float CONVEY_FEE { get; set; }
        [MaxLength(2)] public string CONVEY_TYPE { get; set; }
        [MaxLength(1)] public string SDIR { get; set; }
        public int DIV_DROP { get; set; }
        public int DIV_ORG { get; set; }
        [MaxLength(2)] public string DPW_SANITATION { get; set; }
        public float EXM_ACREAGE { get; set; }
        public float EXM_PER_CT_IMPRV { get; set; }
        public float EXM_PER_CT_LAND { get; set; }
        [MaxLength(1)] public string FIREPLACE { get; set; }
        [MaxLength(2)] public string GARAGE_TYPE { get; set; }
        public int GEO_ALDER { get; set; }
        public int GEO_ALDER_OLD { get; set; }
        public int GEO_BI_MAINT { get; set; }
        [MaxLength(4)] public string GEO_BLOCK { get; set; }
        public int GEO_FIRE { get; set; }
        public int GEO_POLICE { get; set; }
        public int GEO_TRACT { get; set; }
        public int GEO_ZIP_CODE { get; set; }
        [MaxLength(1)] public string HIST_CODE { get; set; }
        public int HOUSE_NR_HI { get; set; }
        public int HOUSE_NR_LO { get; set; }
        [MaxLength(3)] public string HOUSE_NR_SFX { get; set; }
        public int LAND_USE { get; set; }
        public int LAND_USE_GP { get; set; }
        public DateTime LAST_NAME_CHG { get; set; }
        public DateTime LAST_VALUE_CHG { get; set; }
        public int LOT_AREA { get; set; }
        [MaxLength(4)] public string NEIGHBORHOOD { get; set; }
        [MaxLength(4)] public string NR_ROOMS { get; set; }
        public float NR_STORIES { get; set; }
        public int NR_UNITS { get; set; }
        public int NUMBER_OF_SPACES { get; set; }
        [MaxLength(28)] public string OWNER_CITY_STATE { get; set; }
        [MaxLength(28)] public string OWNER_MAIL_ADDR { get; set; }
        [MaxLength(28)] public string OWNER_NAME_1 { get; set; }
        [MaxLength(28)] public string OWNER_NAME_2 { get; set; }
        [MaxLength(28)] public string OWNER_NAME_3 { get; set; }
        [MaxLength(9)] public string OWNER_ZIP { get; set; }
        [MaxLength(1)] public string OWN_OCPD { get; set; }
        [MaxLength(1)] public string P_A_CLASS { get; set; }
        public int P_A_EXM_IMPRV { get; set; }
        public int P_A_EXM_LAND { get; set; }
        public int P_A_EXM_TOTAL { get; set; }
        [MaxLength(3)] public string P_A_EXM_TYPE { get; set; }
        public int P_A_IMPRV { get; set; }
        public int P_A_LAND { get; set; }
        [MaxLength(1)] public string P_A_SYMBOL { get; set; }
        public int P_A_TOTAL { get; set; }
        public int PLAT_PAGE { get; set; }
        public int POWDER_ROOMS { get; set; }
        public int RAZE_STATUS { get; set; }
        [MaxLength(3)] public string REASON_FOR_CHG { get; set; }
        [MaxLength(18)] public string STREET { get; set; }
        [MaxLength(2)] public string STTYPE { get; set; }
        public int SUB_ACCT { get; set; }
        [MaxLength(1)] public string SWIM_POOL { get; set; }
        [MaxLength(2)] public string TAX_RATE_CD { get; set; }
        [MaxLength(4)] public string TOT_UNABATED { get; set; }
        public int YEARS_DELQ { get; set; }
        [MaxLength(4)] public string YR_ASSMT { get; set; }
        public int YR_BUILT { get; set; }
        [MaxLength(7)] public string ZONING { get; set; }
        public int PARKING_SPACES { get; set; }
        [MaxLength(2)] public string PARKING_TYPE { get; set; }
        [MaxLength(2)] public string CORNER_LOT { get; set; }
        public int ANGLE { get; set; }
        public int TAX_DELQ { get; set; }
        [MaxLength(4)] public string BI_VIOL { get; set; }
    }
}
