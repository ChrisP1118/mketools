using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data.Incidents
{
    public class PoliceDispatchCallType : IHasId<string>
    {
        public string GetId() => this.NatureOfCall;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(20)]
        public string NatureOfCall { get; set; }

        public bool IsCritical { get; set; }
        public bool IsViolent { get; set; }
        public bool IsProperty { get; set; }
        public bool IsDrug { get; set; }
        public bool IsTraffic { get; set; }
        public bool IsOtherCrime { get; set; }

        [NotMapped]
        public bool IsMajor
        {
            get
            {
                return IsCritical || IsViolent;
            }
        }

        [NotMapped]
        public bool IsMinor
        {
            get
            {
                return IsProperty || IsDrug || IsOtherCrime;
            }
        }
    }
}
