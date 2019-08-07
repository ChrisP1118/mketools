using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Properties
{
    public class Address : IHasId<string>
    {
        public string GetId() => this.FormattedAddress;

        public void BuildFormattedAddress()
        {
            FormattedAddress = HSE_NBR + SFX + " " + DIR + " " + STREET + " " + STTYPE + " " + UNIT_NBR + " " + ZIP_CODE;
        }

        [MaxLength(10)]
        public string TAXKEY { get; set; }

        [Key]
        [Required]
        [MaxLength(60)]
        public string FormattedAddress { get; set; }

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

        public int RCD_NBR { get; set; }

        public int UPD_DATE { get; set; }

        public int WARD { get; set; }

        public int MAIL_ERROR_COUNT { get; set; }

        [MaxLength(1)]
        public string MAIL_STATUS { get; set; }

        [MaxLength(1)]
        public string RES_COM_FLAG { get; set; }
    }
}
