using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Places;
using NetTopologySuite.Geometries;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Data
{
    public class PropertyService : EntityWriteService<Property, string>
    {
        public PropertyService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<Property> validator, ILogger<EntityWriteService<Property, string>> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<Property>> ApplyIdFilter(IQueryable<Property> queryable, string id)
        {
            return queryable.Where(x => x.TAXKEY == id);
        }

        protected override async Task<IQueryable<Property>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<Property> queryable)
        {
            return queryable;
        }

        protected override async Task<IQueryable<Property>> ApplyBounds(IQueryable<Property> queryable, double northBound, double southBound, double eastBound, double westBound, Polygon bounds, bool useHighPrecision)
        {
            queryable = queryable
                .Where(x => x.Parcel != null)
                .Where(x =>
                    (x.Parcel.CommonParcel.MinLat <= northBound && x.Parcel.CommonParcel.MaxLat >= northBound) ||
                    (x.Parcel.CommonParcel.MinLat <= southBound && x.Parcel.CommonParcel.MaxLat >= southBound) ||
                    (x.Parcel.CommonParcel.MinLat >= northBound && x.Parcel.CommonParcel.MaxLat <= southBound) ||
                    (x.Parcel.CommonParcel.MinLat >= southBound && x.Parcel.CommonParcel.MaxLat <= northBound))
                .Where(x =>
                    (x.Parcel.CommonParcel.MinLng <= westBound && x.Parcel.CommonParcel.MaxLng >= westBound) ||
                    (x.Parcel.CommonParcel.MinLng <= eastBound && x.Parcel.CommonParcel.MaxLng >= eastBound) ||
                    (x.Parcel.CommonParcel.MinLng >= westBound && x.Parcel.CommonParcel.MaxLng <= eastBound) ||
                    (x.Parcel.CommonParcel.MinLng >= eastBound && x.Parcel.CommonParcel.MaxLng <= westBound));

            if (useHighPrecision)
                queryable = queryable
                    .Where(x => x.Parcel.CommonParcel.Outline.Intersects(bounds));

            return queryable;
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, Property dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }
    }
}
