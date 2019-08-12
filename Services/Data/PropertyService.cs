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
                .Where(x => x.Location != null)
                .Where(x => 
                    (x.Location.MinLat <= northBound && x.Location.MaxLat >= northBound) ||
                    (x.Location.MinLat <= southBound && x.Location.MaxLat >= southBound) ||
                    (x.Location.MinLat >= northBound && x.Location.MaxLat <= southBound))
                .Where(x =>
                    (x.Location.MinLng <= westBound && x.Location.MaxLng >= westBound) ||
                    (x.Location.MinLng <= eastBound && x.Location.MaxLng >= eastBound) ||
                    (x.Location.MinLng >= westBound && x.Location.MaxLng <= eastBound))
                .Where(x => x.Location.Outline.Intersects(bounds));
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
            return (await base.GetDataSet(applicationUser)).Include(x => x.Location);
        }
    }
}
