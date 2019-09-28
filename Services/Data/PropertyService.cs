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
        public PropertyService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<Property> validator, ILogger<Property> logger) : base(dbContext, userManager, validator, logger)
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

        protected override async Task<IQueryable<Property>> ApplyBounds(IQueryable<Property> queryable, double northBound, double southBound, double eastBound, double westBound, Polygon bounds)
        {
            return queryable
                .Where(x => x.Parcel != null)
                .Where(x => 
                    (x.Parcel.MinLat <= northBound && x.Parcel.MaxLat >= northBound) ||
                    (x.Parcel.MinLat <= southBound && x.Parcel.MaxLat >= southBound) ||
                    (x.Parcel.MinLat >= northBound && x.Parcel.MaxLat <= southBound) ||
                    (x.Parcel.MinLat >= southBound && x.Parcel.MaxLat <= northBound))
                .Where(x =>
                    (x.Parcel.MinLng <= westBound && x.Parcel.MaxLng >= westBound) ||
                    (x.Parcel.MinLng <= eastBound && x.Parcel.MaxLng >= eastBound) ||
                    (x.Parcel.MinLng >= westBound && x.Parcel.MaxLng <= eastBound) ||
                    (x.Parcel.MinLng >= eastBound && x.Parcel.MaxLng <= westBound))
                .Where(x => x.Parcel.Outline.Intersects(bounds));
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, Property dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }

        protected override async Task<IQueryable<Property>> GetDataSet(ApplicationUser applicationUser)
        {
            return (await base.GetDataSet(applicationUser)).Include(x => x.Parcel);
        }
    }
}
