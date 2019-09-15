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
    public class DispatchCallService : EntityWriteService<DispatchCall, string>
    {
        public DispatchCallService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<DispatchCall> validator, ILogger<DispatchCall> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<DispatchCall>> ApplyIdFilter(IQueryable<DispatchCall> queryable, string id)
        {
            return queryable.Where(x => x.CallNumber == id);
        }

        protected override async Task<IQueryable<DispatchCall>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<DispatchCall> queryable)
        {
            return queryable;
        }

        protected override async Task<IQueryable<DispatchCall>> ApplyBounds(IQueryable<DispatchCall> queryable, double northBound, double southBound, double eastBound, double westBound, Polygon bounds)
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

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, DispatchCall dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }
    }
}
