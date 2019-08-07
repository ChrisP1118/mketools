using FluentValidation;
using Microsoft.AspNetCore.Identity;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Properties;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Data
{
    public class AddressService : EntityWriteService<Address, string>
    {
        public AddressService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<Address> validator) : base(dbContext, userManager, validator)
        {
        }

        protected override async Task<IQueryable<Address>> ApplyIdFilter(IQueryable<Address> queryable, string id)
        {
            return queryable.Where(x => x.FormattedAddress == id);
        }

        protected override async Task<IQueryable<Address>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<Address> queryable)
        {
            return queryable;
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, Address dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }
    }
}
