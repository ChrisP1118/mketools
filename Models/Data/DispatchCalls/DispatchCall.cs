using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.DispatchCalls
{
    public class DispatchCall : IHasId<string>
    {
        public string GetId() => this.CallNumber;

        [Key]
        [Required]
        [MaxLength(12)]
        public string CallNumber { get; set; }

        [Required]
        public DateTime ReportedDateTime { get; set; }

        [Required]
        [MaxLength(60)]
        public string Location { get; set; }

        public int District { get; set; }

        [Required]
        [MaxLength(20)]
        public string NatureOfCall { get; set; }

        [MaxLength(60)]
        public string Status { get; set; }
    }
}
