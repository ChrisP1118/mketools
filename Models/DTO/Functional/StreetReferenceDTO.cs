using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Functional
{
    public class StreetReferenceDTO
    {
        public List<string> StreetDirections { get; set; }
        public List<string> StreetNames { get; set; }
        public List<string> StreetTypes { get; set; }
    }
}
