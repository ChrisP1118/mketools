using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Incidents;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Models.Data.Subscriptions;
using NetTopologySuite.Geometries;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Data
{
    public class DispatchCallSubscriptionService : EntityWriteService<DispatchCallSubscription, Guid>
    {
        public DispatchCallSubscriptionService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<DispatchCallSubscription> validator, ILogger<DispatchCallSubscription> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<DispatchCallSubscription>> ApplyIdFilter(IQueryable<DispatchCallSubscription> queryable, Guid id)
        {
            return queryable.Where(x => x.Id == id);
        }

        protected override async Task<IQueryable<DispatchCallSubscription>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<DispatchCallSubscription> queryable)
        {
            // Site admins can view all
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return queryable;

            // Everyone else can just read their own
            return queryable.Where(x => x.ApplicationUserId == applicationUser.Id);
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, DispatchCallSubscription dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            // Users can write their own
            if (dataModel.ApplicationUserId == applicationUser.Id)
                return true;

            return false;
        }
    }
}
