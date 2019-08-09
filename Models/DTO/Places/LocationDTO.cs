using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Places
{
    public class LocationDTO
    {
        [MaxLength(10)]
        public string TAXKEY { get; set; }
    }
}
