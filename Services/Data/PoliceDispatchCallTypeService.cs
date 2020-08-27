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
    public class PoliceDispatchCallTypeService : EntityWriteService<PoliceDispatchCallType, string>
    {
        public PoliceDispatchCallTypeService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<PoliceDispatchCallType> validator, ILogger<EntityWriteService<PoliceDispatchCallType, string>> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<PoliceDispatchCallType>> ApplyIdFilter(IQueryable<PoliceDispatchCallType> queryable, string id)
        {
            return queryable.Where(x => x.NatureOfCall == id);
        }

        protected override async Task<IQueryable<PoliceDispatchCallType>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<PoliceDispatchCallType> queryable)
        {
            return queryable;
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, PoliceDispatchCallType dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }
    }
}
