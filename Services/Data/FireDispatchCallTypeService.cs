using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Data;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Data.Incidents;
using NetTopologySuite.Geometries;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Data
{
    public class FireDispatchCallTypeService : EntityWriteService<FireDispatchCallType, string>
    {
        public FireDispatchCallTypeService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<FireDispatchCallType> validator, ILogger<EntityWriteService<FireDispatchCallType, string>> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<FireDispatchCallType>> ApplyIdFilter(IQueryable<FireDispatchCallType> queryable, string id)
        {
            return queryable.Where(x => x.NatureOfCall == id);
        }

        protected override async Task<IQueryable<FireDispatchCallType>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<FireDispatchCallType> queryable)
        {
            return queryable;
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, FireDispatchCallType dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }
    }
}
