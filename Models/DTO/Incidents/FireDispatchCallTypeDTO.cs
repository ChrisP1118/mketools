using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.DTO.Incidents
{
    public class FireDispatchCallTypeDTO
    {
        public string NatureOfCall { get; set; }

        public bool IsCritical { get; set; }
        public bool IsFire { get; set; }
        public bool IsMedical { get; set; }

        public bool IsMajor { get; set; }
    }
}
