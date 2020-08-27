using FluentValidation;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Data;
using MkeTools.Web.Jobs;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Data.HistoricPhotos;
using MkeTools.Web.Models.Data.Incidents;
using NetTopologySuite.Geometries;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Data
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

        protected override async Task<IQueryable<HistoricPhoto>> ApplyBounds(IQueryable<HistoricPhoto> queryable, double northBound, double southBound, double eastBound, double westBound, Polygon bounds, bool useHighPrecision)
        {
            queryable = queryable
                .Where(x =>
                    (x.HistoricPhotoLocation.MinLat <= southBound && x.HistoricPhotoLocation.MaxLat >= southBound) ||
                    (x.HistoricPhotoLocation.MinLat <= northBound && x.HistoricPhotoLocation.MaxLat >= northBound) ||
                    (x.HistoricPhotoLocation.MinLat >= southBound && x.HistoricPhotoLocation.MaxLat <= northBound) ||
                    (x.HistoricPhotoLocation.MinLat >= northBound && x.HistoricPhotoLocation.MaxLat <= southBound))
                .Where(x =>
                    (x.HistoricPhotoLocation.MinLng <= westBound && x.HistoricPhotoLocation.MaxLng >= westBound) ||
                    (x.HistoricPhotoLocation.MinLng <= eastBound && x.HistoricPhotoLocation.MaxLng >= eastBound) ||
                    (x.HistoricPhotoLocation.MinLng >= westBound && x.HistoricPhotoLocation.MaxLng <= eastBound) ||
                    (x.HistoricPhotoLocation.MinLng >= eastBound && x.HistoricPhotoLocation.MaxLng <= westBound));

            if (useHighPrecision)
                queryable = queryable
                    .Where(x => x.HistoricPhotoLocation.Geometry.Intersects(bounds));

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
