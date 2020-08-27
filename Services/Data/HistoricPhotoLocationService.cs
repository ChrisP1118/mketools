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
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Data
{
    public class HistoricPhotoLocationService : EntityWriteService<HistoricPhotoLocation, Guid>
    {
        public HistoricPhotoLocationService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<HistoricPhotoLocation> validator, ILogger<EntityWriteService<HistoricPhotoLocation, Guid>> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<HistoricPhotoLocation>> ApplyIdFilter(IQueryable<HistoricPhotoLocation> queryable, Guid id)
        {
            return queryable.Where(x => x.Id == id);
        }

        protected override async Task<IQueryable<HistoricPhotoLocation>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<HistoricPhotoLocation> queryable)
        {
            return queryable;
        }

        protected override async Task<IQueryable<HistoricPhotoLocation>> ApplyBounds(IQueryable<HistoricPhotoLocation> queryable, double northBound, double southBound, double eastBound, double westBound, Polygon bounds, bool useHighPrecision)
        {
            queryable = queryable
                .Where(x =>
                    (x.MinLat <= southBound && x.MaxLat >= southBound) ||
                    (x.MinLat <= northBound && x.MaxLat >= northBound) ||
                    (x.MinLat >= southBound && x.MaxLat <= northBound) ||
                    (x.MinLat >= northBound && x.MaxLat <= southBound))
                .Where(x =>
                    (x.MinLng <= westBound && x.MaxLng >= westBound) ||
                    (x.MinLng <= eastBound && x.MaxLng >= eastBound) ||
                    (x.MinLng >= westBound && x.MaxLng <= eastBound) ||
                    (x.MinLng >= eastBound && x.MaxLng <= westBound));

            if (useHighPrecision)
                queryable = queryable
                    .Where(x => x.Geometry.Intersects(bounds));

            return queryable;
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, HistoricPhotoLocation dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }
    }
}
