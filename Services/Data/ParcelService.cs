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
    public class ParcelService : EntityWriteService<Parcel, string>
    {
        public ParcelService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<Parcel> validator, ILogger<Parcel> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<Parcel>> ApplyIdFilter(IQueryable<Parcel> queryable, string id)
        {
            return queryable.Where(x => x.TAXKEY == id);
        }

        protected override async Task<IQueryable<Parcel>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<Parcel> queryable)
        {
            return queryable;
        }

        protected override async Task<IQueryable<Parcel>> ApplyBounds(IQueryable<Parcel> queryable, double northBound, double southBound, double eastBound, double westBound, Polygon bounds)
        {
            return queryable
                .Where(x =>
                    (x.MinLat <= northBound && x.MaxLat >= northBound) ||
                    (x.MinLat <= southBound && x.MaxLat >= southBound) ||
                    (x.MinLat >= northBound && x.MaxLat <= southBound) ||
                    (x.MinLat >= southBound && x.MaxLat <= northBound))
                .Where(x =>
                    (x.MinLng <= westBound && x.MaxLng >= westBound) ||
                    (x.MinLng <= eastBound && x.MaxLng >= eastBound) ||
                    (x.MinLng >= westBound && x.MaxLng <= eastBound) ||
                    (x.MinLng >= eastBound && x.MaxLng <= westBound))
                .Where(x => x.Outline.Intersects(bounds));
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, Parcel dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }
    }
}