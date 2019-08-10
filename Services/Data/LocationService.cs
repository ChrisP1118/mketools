using FluentValidation;
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
            //return queryable;
            Point myLocation = new Point(-87.885001526424077, 43.06512940428771)
            {
                SRID = 4326
            };

            Polygon bounds = new Polygon(new LinearRing(new Coordinate[]
            {
                new Coordinate(-87.88540495881335, 43.06530806130551), // NW
                new Coordinate(-87.88540495881335, 43.06381877978135), // SW
                new Coordinate(-87.88293330320613, 43.06381877978135), // SE
                new Coordinate(-87.88293330320613, 43.06530806130551), // NE
                new Coordinate(-87.88540495881335, 43.06530806130551), // NW
            }));
            bounds.SRID = 4326;
            //ne: { lat: 43.06381877978135, lng: -87.88540495881335}
            //sw: { lat: 43.06530806130551, lng: -87.88293330320613}
            //__proto__: Object

            return queryable.Where(x => x.Outline.Overlaps(bounds));
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
