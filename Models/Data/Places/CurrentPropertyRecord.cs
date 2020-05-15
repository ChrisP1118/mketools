using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Places
{
    public class CurrentPropertyRecord
    {
        public string TAXKEY { get; set; }
        public DateTime? LAST_NAME_CHG { get; set; }
        public DateTime? LAST_VALUE_CHG { get; set; }
    }
}
