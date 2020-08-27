using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Data;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Data.Incidents;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Models.Data.Subscriptions;
using NetTopologySuite.Geometries;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Data
{
    public class PickupDatesSubscriptionService : EntityWriteService<PickupDatesSubscription, Guid>
    {
        public PickupDatesSubscriptionService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<PickupDatesSubscription> validator, ILogger<EntityWriteService<PickupDatesSubscription, Guid>> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<PickupDatesSubscription>> ApplyIdFilter(IQueryable<PickupDatesSubscription> queryable, Guid id)
        {
            return queryable.Where(x => x.Id == id);
        }

        protected override async Task<IQueryable<PickupDatesSubscription>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<PickupDatesSubscription> queryable)
        {
            // Site admins can view all
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return queryable;

            // Everyone else can just read their own
            return queryable.Where(x => x.ApplicationUserId == applicationUser.Id);
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, PickupDatesSubscription dataModel)
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
