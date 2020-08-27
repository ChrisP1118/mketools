using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data
{
    public interface IGeocodable : IHasBounds
    {
        GeometryAccuracy? Accuracy { get; set; }
        GeometrySource? Source { get; set; }
        DateTime? LastGeocodeAttempt { get; set; }
    }
}
