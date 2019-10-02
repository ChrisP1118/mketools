using MkeAlerts.Web.Filters.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data
{
    [Flags]
    public enum DispatchCallType : int
    {
        PoliceDispatchCall = 1,
        FireDispatchCall = 2,
        AllDispatchCall = PoliceDispatchCall | FireDispatchCall,
        MajorPoliceDispatchCall = 4,
        MajorFireDispatchCall = 8,
        MajorCall = MajorPoliceDispatchCall | MajorFireDispatchCall
    }
}
