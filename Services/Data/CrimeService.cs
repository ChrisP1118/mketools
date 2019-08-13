﻿using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Incidents;
using MkeAlerts.Web.Models.Data.Places;
using NetTopologySuite.Geometries;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Data
{
    public class CrimeService : EntityWriteService<Crime, string>
    {
        public CrimeService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<Crime> validator, ILogger<Crime> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<Crime>> ApplyIdFilter(IQueryable<Crime> queryable, string id)
        {
            //return queryable.Where(x => x.FormattedAddress == id);
            return queryable.Where(x => x.IncidentNum == id);
        }

        protected override async Task<IQueryable<Crime>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<Crime> queryable)
        {
            return queryable;
        }

        protected override async Task<IQueryable<Crime>> ApplyBounds(IQueryable<Crime> queryable, double northBound, double southBound, double eastBound, double westBound, Polygon bounds)
        {
            return queryable
                .Where(x =>
                    (x.MinLat <= northBound && x.MaxLat >= northBound) ||
                    (x.MinLat <= southBound && x.MaxLat >= southBound) ||
                    (x.MinLat >= northBound && x.MaxLat <= southBound) ||
                    (x.MinLat >= southBound && x.MaxLat <= southBound))
                .Where(x =>
                    (x.MinLng <= westBound && x.MaxLng >= westBound) ||
                    (x.MinLng <= eastBound && x.MaxLng >= eastBound) ||
                    (x.MinLng >= westBound && x.MaxLng <= eastBound) ||
                    (x.MinLng >= eastBound && x.MaxLng <= westBound))
                .Where(x => x.Point.Intersects(bounds));
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, Crime dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }
    }
}
