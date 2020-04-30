using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.AppHealth;
using MkeAlerts.Web.Services.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Data
{
    public class JobRunService : EntityWriteService<JobRun, Guid>, IJobRunService
    {
        public JobRunService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<JobRun> validator, ILogger<JobRunService> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<JobRun>> ApplyIdFilter(IQueryable<JobRun> queryable, Guid id)
        {
            return queryable.Where(x => x.Id == id);
        }

        protected override async Task<IQueryable<JobRun>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<JobRun> queryable)
        {
            // Site admins can read everything
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return queryable;

            return queryable.Where(x => false);
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, JobRun dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }

    }
}
