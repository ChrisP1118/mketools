using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Incidents
{
    public class DispatchCallDTO
    {
        public string CallNumber { get; set; }

        public DateTime ReportedDateTime { get; set; }
        public string Location { get; set; }
        public int District { get; set; }
        public string NatureOfCall { get; set; }
        public string Status { get; set; }
    }
}
