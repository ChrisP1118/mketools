using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Models.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Functional
{
    public class StreetReferenceService : IStreetReferenceService
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly IMemoryCache _memoryCache;

        public StreetReferenceService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _memoryCache = memoryCache;
        }

        public async Task<List<string>> GetAllStreetDirections(ClaimsPrincipal user, string municipality)
        {
            return await _memoryCache.GetOrCreateAsync("StreetReferenceService.AllStreetDirections." + municipality, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(6);
                return await GetAllValues(user, "SELECT COALESCE(Dir, '') AS Value FROM Addresses WHERE Muni = {0} GROUP BY Dir ORDER BY Dir", municipality);
            });
        }

        public async Task<List<string>> GetAllStreetNames(ClaimsPrincipal user, string municipality)
        {
            return await _memoryCache.GetOrCreateAsync("StreetReferenceService.AllStreetNames." + municipality, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(6);
                return await GetAllValues(user, "SELECT COALESCE(Street, '') AS Value FROM Addresses WHERE Muni = {0} GROUP BY Street ORDER BY CASE WHEN PATINDEX('%[^0-9]%', Street) = 1 THEN 1 ELSE 0 END, CASE WHEN LEN(Street) < 2 THEN 0 ELSE CAST(SUBSTRING(Street, 1, PATINDEX('%[^0-9]%', Street) - 1) AS int) END, Street", municipality);
            });
        }

        public async Task<List<string>> GetAllStreetTypes(ClaimsPrincipal user, string municipality)
        {
            return await _memoryCache.GetOrCreateAsync("StreetReferenceService.AllStreetTypes." + municipality, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(6);
                return await GetAllValues(user, "SELECT COALESCE(StType, '') AS Value FROM Addresses WHERE Muni = {0} GROUP BY StType ORDER BY StType", municipality);
            });
        }

        protected async Task<List<string>> GetAllValues(ClaimsPrincipal user, string query, params object[] parameters)
        {
            IQueryable<StringReference> queryable = _dbContext.StreetNames.FromSqlRaw(query, parameters);

            List<StringReference> items = await queryable.ToListAsync();
            return items.Select(i => i.Value).ToList();
        }
    }
}
