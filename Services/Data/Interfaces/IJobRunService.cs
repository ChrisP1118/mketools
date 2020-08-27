using MkeTools.Web.Models.Data.AppHealth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Data.Interfaces
{
    public interface IJobRunService : IEntityWriteService<JobRun, Guid>
    {
    }
}
