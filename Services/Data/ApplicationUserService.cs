﻿using Microsoft.AspNetCore.Identity;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Models.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Data
{
    public class ApplicationUserService : EntityReadService<ApplicationUser, Guid>
    {
        public ApplicationUserService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) : base(dbContext, userManager)
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
