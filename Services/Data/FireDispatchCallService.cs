using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Incidents;
using NetTopologySuite.Geometries;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Data
{
    public class FireDispatchCallService : EntityWriteService<FireDispatchCall, string>
    {
        public FireDispatchCallService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<FireDispatchCall> validator, ILogger<EntityWriteService<FireDispatchCall, string>> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<FireDispatchCall>> ApplyIdFilter(IQueryable<FireDispatchCall> queryable, string id)
        {
            return queryable.Where(x => x.CFS == id);
        }

        protected override async Task<IQueryable<FireDispatchCall>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<FireDispatchCall> queryable)
        {
            return queryable;
        }

        protected override async Task<IQueryable<FireDispatchCall>> ApplyBounds(IQueryable<FireDispatchCall> queryable, double northBound, double southBound, double eastBound, double westBound, Polygon bounds, bool useHighPrecision)
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

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, FireDispatchCall dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }
    }
}
