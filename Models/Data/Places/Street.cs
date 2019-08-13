using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Places
{
    public class Street : IHasId<string>, IHasBounds
    {
        public string GetId() => this.NEWDIME_ID;

        //[Column(TypeName = "geometry")]
        public IGeometry Outline { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public double MinLat { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public double MaxLat { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public double MinLng { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public double MaxLng { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(10)]
        public string NEWDIME_ID { get; set; }

        public int FNODE { get; set; }
        public int TNODE { get; set; }
        public int LPOLY { get; set; }
        public int RPOLY { get; set; }
        public double LENGTH { get; set; }
        public int NEWDIMENR { get; set; }
        //public int NEWDIME_ID { get; set; }
        public int RCD_NBR { get; set; }
        public int TRANS_ID { get; set; }
        public int SQUAD_L { get; set; }
        public int BOILER_L { get; set; }
        public int BI_CONST_L { get; set; }
        public int BI_ELECT_L { get; set; }
        public int BI_ELEV_L { get; set; }
        public int BI_PLUMB_L { get; set; }
        public int BI_SPRINK_ { get; set; }
        public int BI_CNDMN_L { get; set; }
        [MaxLength(15)] public string CNTYNAME_L { get; set; }
        public int CNTY_L { get; set; }
        [MaxLength(15)] public string MUNI_L { get; set; }
        public int FMCD_L { get; set; }
        [MaxLength(4)] public string FBLOCK_L { get; set; }
        [MaxLength(6)] public string FTRACT_L { get; set; }
        public int ZIP_L { get; set; }
        [MaxLength(5)] public string QTR_SECT_L { get; set; }
        public int WW_PRES_L { get; set; }
        public int WW_SERV_L { get; set; }
        public int MPS_ELEM_L { get; set; }
        public int MPS_MS_L { get; set; }
        public int MPS_HS_L { get; set; }
        public int POLICE_L { get; set; }
        [MaxLength(10)] public string ST_MAIN_L { get; set; }
        [MaxLength(2)] public string SAN_DIST_L { get; set; }
        public int FOR_TR_L { get; set; }
        public int FOR_BL_L { get; set; }
        [MaxLength(12)] public string SUM_RT_L { get; set; }
        [MaxLength(3)] public string SUM_DA_L { get; set; }
        public int WARD2K_L { get; set; }
        public int TRACT2K_L { get; set; }
        [MaxLength(4)] public string BLOCK2K_L { get; set; }
        public int CONGR2K_L { get; set; }
        public int STSEN2K_L { get; set; }
        public int STASS2K_L { get; set; }
        public int CSUP2K_L { get; set; }
        public int FIREBAT_L { get; set; }
        public int SCHOOL2K_L { get; set; }
        public int POLRD_L { get; set; }
        public int ALD2004_L { get; set; }
        [MaxLength(12)] public string WIN_RT_L { get; set; }
        [MaxLength(20)] public string RECYC_SM_L { get; set; }
        [MaxLength(3)] public string MUNICODE_L { get; set; }
        public int WW_ROUT_L { get; set; }
        [MaxLength(3)] public string RECYC_DA_L { get; set; }
        [MaxLength(20)] public string RECYC_WN_L { get; set; }
        [MaxLength(5)] public string WTR16TH_L { get; set; }
        [MaxLength(5)] public string SANLEAF_L { get; set; }
        [MaxLength(3)] public string SANPLOW_L { get; set; }
        [MaxLength(5)] public string BROOM_L { get; set; }
        [MaxLength(5)] public string BROOMALL_L { get; set; }
        [MaxLength(8)] public string LOCDIST_L { get; set; }
        public int FOODINSP_L { get; set; }
        [MaxLength(15)] public string CIPAREA_L { get; set; }
        public int TRACT_L { get; set; }
        public int ALD_L { get; set; }
        public int WARD_L { get; set; }
        public int SCHOOL_L { get; set; }
        [MaxLength(4)] public string BLOCK_L { get; set; }
        public int STASS_L { get; set; }
        public int STSEN_L { get; set; }
        public int CNTYSUP_L { get; set; }
        [MaxLength(3)] public string COMBSEW_L { get; set; }
        [MaxLength(10)] public string SANBIZPL_L { get; set; }
        public int ST_OP_L { get; set; }
        public int FOR_PM_L { get; set; }
        [MaxLength(50)] public string CONSERVE_L { get; set; }
        public int SQUAD_R { get; set; }
        public int BOILER_R { get; set; }
        public int BI_CONST_R { get; set; }
        public int BI_ELECT_R { get; set; }
        public int BI_ELEV_R { get; set; }
        public int BI_PLUMB_R { get; set; }
        public int SPRINK_R { get; set; }
        public int BI_CNDMN_R { get; set; }
        [MaxLength(15)] public string CNTYNAME_R { get; set; }
        public int CNTY_R { get; set; }
        [MaxLength(15)] public string MUNI_R { get; set; }
        public int FMCD_R { get; set; }
        [MaxLength(4)] public string FBLOCK_R { get; set; }
        [MaxLength(6)] public string FTRACT_R { get; set; }
        public int ZIP_R { get; set; }
        [MaxLength(5)] public string QTR_SECT_R { get; set; }
        public int WW_PRES_R { get; set; }
        public int WW_SERV_R { get; set; }
        public int MPS_ELEM_R { get; set; }
        public int MPS_MS_R { get; set; }
        public int MPS_HS_R { get; set; }
        public int POLICE_R { get; set; }
        [MaxLength(10)] public string ST_MAIN_R { get; set; }
        [MaxLength(2)] public string SAN_DIST_R { get; set; }
        public int FOR_TR_R { get; set; }
        public int FOR_BL_R { get; set; }
        [MaxLength(12)] public string SUM_RT_R { get; set; }
        [MaxLength(3)] public string SUM_DA_R { get; set; }
        public int WARD2K_R { get; set; }
        public int TRACT2K_R { get; set; }
        [MaxLength(4)] public string BLOCK2K_R { get; set; }
        public int CONGR2K_R { get; set; }
        public int STSEN2K_R { get; set; }
        public int STASS2K_R { get; set; }
        public int CSUP2K_R { get; set; }
        public int FIREBAT_R { get; set; }
        public int SCHOOL2K_R { get; set; }
        public int POLRD_R { get; set; }
        public int ALD2004_R { get; set; }
        [MaxLength(12)] public string WIN_RT_R { get; set; }
        [MaxLength(20)] public string RECYC_SM_R { get; set; }
        [MaxLength(3)] public string MUNICODE_R { get; set; }
        public int WW_ROUT_R { get; set; }
        [MaxLength(3)] public string RECYC_DA_R { get; set; }
        [MaxLength(20)] public string RECYC_WN_R { get; set; }
        [MaxLength(5)] public string WTR16TH_R { get; set; }
        [MaxLength(5)] public string SANLEAF_R { get; set; }
        [MaxLength(3)] public string SANPLOW_R { get; set; }
        [MaxLength(5)] public string BROOM_R { get; set; }
        [MaxLength(5)] public string BROOMALL_R { get; set; }
        [MaxLength(8)] public string LOCDIST_R { get; set; }
        public int FOODINSP_R { get; set; }
        [MaxLength(15)] public string CIPAREA_R { get; set; }
        public int TRACT_R { get; set; }
        public int ALD_R { get; set; }
        public int WARD_R { get; set; }
        public int SCHOOL_R { get; set; }
        [MaxLength(4)] public string BLOCK_R { get; set; }
        public int STASS_R { get; set; }
        public int STSEN_R { get; set; }
        public int CNTYSUP_R { get; set; }
        [MaxLength(3)] public string COMBSEW_R { get; set; }
        [MaxLength(10)] public string SANBIZPL_R { get; set; }
        public int ST_OP_R { get; set; }
        public int FOR_PM_R { get; set; }
        [MaxLength(50)] public string CONSERVE_R { get; set; }
        [MaxLength(10)] public string SEG_L_TYPE { get; set; }
        public int LEVEL { get; set; }
        [MaxLength(1)] public string DIR { get; set; }
        [MaxLength(18)] public string STREET { get; set; }
        [MaxLength(2)] public string STTYPE { get; set; }
        public int LO_ADD_L { get; set; }
        public int HI_ADD_L { get; set; }
        public int LO_ADD_R { get; set; }
        public int HI_ADD_R { get; set; }
        [MaxLength(10)] public string BUS_L { get; set; }
        [MaxLength(10)] public string BUS_R { get; set; }
        public int STCLASS { get; set; }
        [MaxLength(3)] public string CFCC { get; set; }
        public int FROM_NODE { get; set; }
        public int TO_NODE { get; set; }
        public double LOW_X { get; set; }
        public double LOW_Y { get; set; }
        public double HIGH_X { get; set; }
        public double HIGH_Y { get; set; }

    }
}
