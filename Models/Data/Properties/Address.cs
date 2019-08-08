using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Properties
{
    public class Address : IHasId<string>
    {
        public string GetId() => this.RCD_NBR;

        [MaxLength(10)]
        public string TAXKEY { get; set; }

        public int HSE_NBR { get; set; }

        [MaxLength(3)]
        public string SFX { get; set; }

        [MaxLength(1)]
        public string DIR { get; set; }

        [MaxLength(18)]
        public string STREET { get; set; }

        [MaxLength(2)]
        public string STTYPE { get; set; }

        [MaxLength(5)]
        public string UNIT_NBR { get; set; }

        [MaxLength(9)]
        public string ZIP_CODE { get; set; }

        public int LAND_USE { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(10)]
        public string RCD_NBR { get; set; }

        public int UPD_DATE { get; set; }

        public int WARD { get; set; }

        public int MAIL_ERROR_COUNT { get; set; }

        [MaxLength(1)]
        public string MAIL_STATUS { get; set; }

        [MaxLength(1)]
        public string RES_COM_FLAG { get; set; }
    }
}
