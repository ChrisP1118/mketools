using FluentValidation;
using Microsoft.AspNetCore.Identity;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.DispatchCalls;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Data
{
    public class DispatchCallService : EntityWriteService<DispatchCall, string>
    {
        public DispatchCallService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<DispatchCall> validator) : base(dbContext, userManager, validator)
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

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, DispatchCall dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }
    }
}
