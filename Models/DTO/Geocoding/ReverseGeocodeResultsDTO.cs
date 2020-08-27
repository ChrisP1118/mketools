using MkeTools.Web.Models.DTO.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.DTO.Geocoding
{
    public class ReverseGeocodeResultsDTO
    {
        public CommonParcelDTO CommonParcel { get; set; }
        public double Distance { get; set; }
    }
}
