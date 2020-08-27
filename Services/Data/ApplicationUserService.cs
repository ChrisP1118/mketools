using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Data;
using MkeTools.Web.Models.Data;
using MkeTools.Web.Models.Data.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Data
{
    public class ApplicationUserService : EntityReadService<ApplicationUser, Guid>
    {
        public ApplicationUserService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, ILogger<EntityReadService<ApplicationUser, Guid>> logger) : base(dbContext, userManager, logger)
        {
        }

        protected override async Task<IQueryable<ApplicationUser>> ApplyIdFilter(IQueryable<ApplicationUser> queryable, Guid id)
        {
            return queryable.Where(x => x.Id == id);
        }

        protected override async Task<IQueryable<ApplicationUser>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<ApplicationUser> queryable)
        {
            // Site admins can read all users
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return queryable;

            return queryable.Where(x => false);
        }
    }
}
