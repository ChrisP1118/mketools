using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Data;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Services.Data.Interfaces;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Data
{
    public class ParcelService : EntityWriteService<Parcel, string>, IParcelService
    {
        public ParcelService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<Parcel> validator, ILogger<EntityWriteService<Parcel, string>> logger) : base(dbContext, userManager, validator, logger)
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

        protected override async Task<IQueryable<Parcel>> ApplyBounds(IQueryable<Parcel> queryable, double northBound, double southBound, double eastBound, double westBound, Polygon bounds, bool useHighPrecision)
        {
            queryable = queryable
                .Where(x =>
                    (x.CommonParcel.MinLat <= northBound && x.CommonParcel.MaxLat >= northBound) ||
                    (x.CommonParcel.MinLat <= southBound && x.CommonParcel.MaxLat >= southBound) ||
                    (x.CommonParcel.MinLat >= northBound && x.CommonParcel.MaxLat <= southBound) ||
                    (x.CommonParcel.MinLat >= southBound && x.CommonParcel.MaxLat <= northBound))
                .Where(x =>
                    (x.CommonParcel.MinLng <= westBound && x.CommonParcel.MaxLng >= westBound) ||
                    (x.CommonParcel.MinLng <= eastBound && x.CommonParcel.MaxLng >= eastBound) ||
                    (x.CommonParcel.MinLng >= westBound && x.CommonParcel.MaxLng <= eastBound) ||
                    (x.CommonParcel.MinLng >= eastBound && x.CommonParcel.MaxLng <= westBound));

            if (useHighPrecision)
                queryable = queryable
                    .Where(x => x.CommonParcel.Outline.Intersects(bounds));

            return queryable;
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, Parcel dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }

        public async Task<HashSet<string>> GetAllTaxkeys(ClaimsPrincipal user)
        {
            var applicationUser = await GetApplicationUser(user);

            List<string> allTaxkeys = await (await GetDataSet(applicationUser))
                .Select(x => x.TAXKEY)
                .AsNoTracking()
                .ToListAsync();

            return allTaxkeys.ToHashSet<string>();
        }
    }
}
