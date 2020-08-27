using MkeTools.Web.Models.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Functional
{
    public interface IGeocodingService
    {
        Task<GeocodeResults> Geocode(string value);
        Task<ReverseGeocodeResults> ReverseGeocode(double latitude, double longitude);
    }

}
