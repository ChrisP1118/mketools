using FluentValidation;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Data;
using MkeTools.Web.Jobs;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Data.Incidents;
using NetTopologySuite.Geometries;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Data
{
    public class PoliceDispatchCallService : EntityWriteService<PoliceDispatchCall, string>
    {
        public PoliceDispatchCallService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<PoliceDispatchCall> validator, ILogger<EntityWriteService<PoliceDispatchCall, string>> logger) : base(dbContext, userManager, validator, logger)
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

        protected override async Task<IQueryable<PoliceDispatchCall>> ApplyBounds(IQueryable<PoliceDispatchCall> queryable, double northBound, double southBound, double eastBound, double westBound, Polygon bounds, bool useHighPrecision)
        {
            queryable = queryable
                .Where(x =>
                    (x.MinLat <= southBound && x.MaxLat >= southBound) ||
                    (x.MinLat <= northBound && x.MaxLat >= northBound) ||
                    (x.MinLat >= southBound && x.MaxLat <= northBound) ||
                    (x.MinLat >= northBound && x.MaxLat <= southBound))
                .Where(x =>
                    (x.MinLng <= westBound && x.MaxLng >= westBound) ||
                    (x.MinLng <= eastBound && x.MaxLng >= eastBound) ||
                    (x.MinLng >= westBound && x.MaxLng <= eastBound) ||
                    (x.MinLng >= eastBound && x.MaxLng <= westBound));

            if (useHighPrecision)
                queryable = queryable
                    .Where(x => x.Geometry.Intersects(bounds));

            return queryable;
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
