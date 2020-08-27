using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data.AppHealth
{
    public class JobRun : IHasId<Guid>
    {
        public Guid GetId() => this.Id;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public Guid Id { get; set; }

        [MaxLength(40)]
        public string JobName { get; set; }

        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }

        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }

        public string ErrorMessages { get; set; }
        public string ErrorStackTrace { get; set; }
    }
}
