using MkeTools.Web.Filters.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data
{
    [Flags]
    public enum DispatchCallType : int
    {
        JustNonCrimePoliceDispatchCall = 1,
        JustMinorPoliceDispatchCall = 2,
        JustMajorPoliceDispatchCall = 4,

        MinorOrMajorPoliceDispatchCall = JustMinorPoliceDispatchCall | JustMajorPoliceDispatchCall,
        AnyPoliceDispatchCall = JustNonCrimePoliceDispatchCall | JustMinorPoliceDispatchCall | JustMajorPoliceDispatchCall,

        JustMinorFireDispatchCall = 8,
        JustMajorFireDispatchCall = 16,
        AnyFireDispatchCall = JustMinorFireDispatchCall | JustMajorFireDispatchCall,
        
        AnyMajorDispatchCall = JustMajorPoliceDispatchCall | JustMajorFireDispatchCall,
        AnyDispatchCall = JustNonCrimePoliceDispatchCall | JustMinorPoliceDispatchCall | JustMajorPoliceDispatchCall | JustMinorFireDispatchCall | JustMajorFireDispatchCall
    }
}
