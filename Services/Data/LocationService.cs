﻿using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Places;
using NetTopologySuite.Geometries;
using System.Linq;
using System.Threading.Tasks;
using Coordinate = GeoAPI.Geometries.Coordinate;

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

        protected override async Task<IQueryable<Location>> ApplyBounds(IQueryable<Location> queryable, double northBound, double southBound, double eastBound, double westBound, Polygon bounds)
        {
            return queryable
                .Where(x =>
                    (x.MinLat <= northBound && x.MaxLat >= northBound) ||
                    (x.MinLat <= southBound && x.MaxLat >= southBound) ||
                    (x.MinLat >= northBound && x.MaxLat <= southBound))
                .Where(x =>
                    (x.MinLng <= westBound && x.MaxLng >= westBound) ||
                    (x.MinLng <= eastBound && x.MaxLng >= eastBound) ||
                    (x.MinLng >= westBound && x.MaxLng <= eastBound))
                .Where(x => x.Outline.Intersects(bounds));
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
