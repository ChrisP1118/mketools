using FluentValidation;
using Microsoft.AspNetCore.Identity;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Properties;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Data
{
    public class PropertyService : EntityWriteService<Property, string>
    {
        public PropertyService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<Property> validator) : base(dbContext, userManager, validator)
        {
        }

        protected override async Task<IQueryable<Property>> ApplyIdFilter(IQueryable<Property> queryable, string id)
        {
            return queryable.Where(x => x.TAXKEY == id);
        }

        protected override async Task<IQueryable<Property>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<Property> queryable)
        {
            return queryable;
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, Property dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }
    }
}
