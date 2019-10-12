using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Incidents
{
    public class PoliceDispatchCallTypeDTO
    {
        public string NatureOfCall { get; set; }

        public bool IsCritical { get; set; }
        public bool IsViolent { get; set; }
        public bool IsProperty { get; set; }
        public bool IsDrug { get; set; }
        public bool IsTraffic { get; set; }
        public bool IsOtherCrime { get; set; }

        public bool IsMajor { get; set; }
        public bool IsMinor { get; set; }
    }
}
