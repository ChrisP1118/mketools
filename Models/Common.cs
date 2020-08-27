using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models
{
    public enum GeometryAccuracy : int
    {
        NoGeometry = 0,
        Low = 10,
        Medium = 20,
        High = 30
    }

    public enum GeometrySource : int
    {
        NoGeometry = 0,
        ExactAddress = 1,
        ExactParcel = 2,
        NearbyAddress = 32,
        NearbyParcel = 33,
        NearbyStreet = 34,
        //AddressAndLocation = 1,
        //PropertyAndLocation = 2,
        //AddressBlock = 3,
        StreetIntersection = 128
    }
}
