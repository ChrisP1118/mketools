using MkeAlerts.Web.Models.Data.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Internal
{
    public class ReverseGeocodeResults
    {
        public CommonParcel CommonParcel { get; set; }
        public double Distance { get; set; }
    }
}
