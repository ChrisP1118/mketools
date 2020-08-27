using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Data;
using MkeTools.Web.Exceptions;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Services.Data.Interfaces;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Data
{
    public class CommonParcelService : EntityWriteService<CommonParcel, int>, ICommonParcelService
    {
        public CommonParcelService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<CommonParcel> validator, ILogger<EntityWriteService<CommonParcel, int>> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<CommonParcel>> ApplyIdFilter(IQueryable<CommonParcel> queryable, int id)
        {
            return queryable.Where(x => x.MAP_ID == id);
        }

        protected override async Task<IQueryable<CommonParcel>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<CommonParcel> queryable)
        {
            return queryable;
        }

        protected override async Task<IQueryable<CommonParcel>> ApplyBounds(IQueryable<CommonParcel> queryable, double northBound, double southBound, double eastBound, double westBound, Polygon bounds, bool useHighPrecision)
        {
            queryable = queryable
                .Where(x =>
                    (x.MinLat <= northBound && x.MaxLat >= northBound) ||
                    (x.MinLat <= southBound && x.MaxLat >= southBound) ||
                    (x.MinLat >= northBound && x.MaxLat <= southBound) ||
                    (x.MinLat >= southBound && x.MaxLat <= northBound))
                .Where(x =>
                    (x.MinLng <= westBound && x.MaxLng >= westBound) ||
                    (x.MinLng <= eastBound && x.MaxLng >= eastBound) ||
                    (x.MinLng >= westBound && x.MaxLng <= eastBound) ||
                    (x.MinLng >= eastBound && x.MaxLng <= westBound));

            if (useHighPrecision)
                queryable = queryable
                    .Where(x => x.Outline.Intersects(bounds));

            return queryable;
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, CommonParcel dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }
    }
}
