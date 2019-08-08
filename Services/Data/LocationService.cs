using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Properties;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Data
{
    public class LocationService : EntityWriteService<Location, string>
    {
        public LocationService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<Location> validator, ILogger<Location> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<Location>> ApplyIdFilter(IQueryable<Location> queryable, string id)
        {
            return queryable.Where(x => x.TAXKEY == id);
        }

        protected override async Task<IQueryable<Location>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<Location> queryable)
        {
            return queryable;
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, Location dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }
    }
}
