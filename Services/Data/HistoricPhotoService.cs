using FluentValidation;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Jobs;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.HistoricPhotos;
using MkeAlerts.Web.Models.Data.Incidents;
using NetTopologySuite.Geometries;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Data
{
    public class HistoricPhotoService : EntityWriteService<HistoricPhoto, string>
    {
        public HistoricPhotoService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<HistoricPhoto> validator, ILogger<EntityWriteService<HistoricPhoto, string>> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<HistoricPhoto>> ApplyIdFilter(IQueryable<HistoricPhoto> queryable, string id)
        {
            return queryable.Where(x => x.Id == id);
        }

        protected override async Task<IQueryable<HistoricPhoto>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<HistoricPhoto> queryable)
        {
            return queryable;
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, HistoricPhoto dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }
    }
}
