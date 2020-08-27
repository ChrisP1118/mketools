using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data.Incidents
{
    public class FireDispatchCallType : IHasId<string>
    {
        public string GetId() => this.NatureOfCall;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(40)]
        public string NatureOfCall { get; set; }

        public bool IsCritical { get; set; }
        public bool IsFire { get; set; }
        public bool IsMedical { get; set; }

        [NotMapped]
        public bool IsMajor
        {
            get
            {
                return IsCritical || IsFire;
            }
        }
    }
}
