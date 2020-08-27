using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Data;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Data.Places;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Data
{
    public class AddressService : EntityWriteService<Address, int>
    {
        public AddressService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<Address> validator, ILogger<EntityWriteService<Address, int>> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<Address>> ApplyIdFilter(IQueryable<Address> queryable, int id)
        {
            return queryable.Where(x => x.ADDRESS_ID == id);
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
