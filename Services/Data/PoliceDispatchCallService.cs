using FluentValidation;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Jobs;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Incidents;
using NetTopologySuite.Geometries;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Data
{
    public class PoliceDispatchCallService : EntityWriteService<PoliceDispatchCall, string>
    {
        public PoliceDispatchCallService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<PoliceDispatchCall> validator, ILogger<PoliceDispatchCall> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<PoliceDispatchCall>> ApplyIdFilter(IQueryable<PoliceDispatchCall> queryable, string id)
        {
            return queryable.Where(x => x.CallNumber == id);
        }

        protected override async Task<IQueryable<PoliceDispatchCall>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<PoliceDispatchCall> queryable)
        {
            return queryable;
        }

        protected override async Task<IQueryable<PoliceDispatchCall>> ApplyBounds(IQueryable<PoliceDispatchCall> queryable, double northBound, double southBound, double eastBound, double westBound, Polygon bounds)
        {
            return queryable
            .Where(x =>
                (x.MinLat <= southBound && x.MaxLat >= southBound) ||
                (x.MinLat <= northBound && x.MaxLat >= northBound) ||
                (x.MinLat >= southBound && x.MaxLat <= northBound) ||
                (x.MinLat >= northBound && x.MaxLat <= southBound))
            .Where(x =>
                (x.MinLng <= westBound && x.MaxLng >= westBound) ||
                (x.MinLng <= eastBound && x.MaxLng >= eastBound) ||
                (x.MinLng >= westBound && x.MaxLng <= eastBound) ||
                (x.MinLng >= eastBound && x.MaxLng <= westBound))
            .Where(x => x.Geometry.Intersects(bounds));
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, PoliceDispatchCall dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }

        protected override async Task OnCreated(PoliceDispatchCall dataModel)
        {
            BackgroundJob.Enqueue<SendPoliceDispatchCallNotifications>(x => x.Run(dataModel.GetId()));
        }
    }
}
