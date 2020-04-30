using MkeAlerts.Web.Models.Data.AppHealth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Data.Interfaces
{
    public interface IJobRunService : IEntityWriteService<JobRun, Guid>
    {
    }
}
