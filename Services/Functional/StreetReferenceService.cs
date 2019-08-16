using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public StreetReferenceService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<List<string>> GetAllStreetDirections(ClaimsPrincipal user)
        {
            return await GetAllValues(user, "SELECT Dir AS Value FROM Addresses GROUP BY Dir ORDER BY Dir");
        }

        public async Task<List<string>> GetAllStreetNames(ClaimsPrincipal user)
        {
            return await GetAllValues(user, "SELECT Street AS Value FROM Addresses GROUP BY Street ORDER BY CASE WHEN PATINDEX('%[^0-9]%', Street) = 1 THEN 1 ELSE 0 END, CAST(SUBSTRING(Street, 1, PATINDEX('%[^0-9]%', Street) - 1) AS int), Street");
        }

        public async Task<List<string>> GetAllStreetTypes(ClaimsPrincipal user)
        {
            return await GetAllValues(user, "SELECT StType AS Value FROM Addresses GROUP BY StType ORDER BY StType");
        }

        protected async Task<List<string>> GetAllValues(ClaimsPrincipal user, string query)
        {
            IQueryable<StringReference> queryable = _dbContext.StreetNames.FromSql(query);

            List<StringReference> items = await queryable.ToListAsync();
            return items.Select(i => i.Value).ToList();
        }
    }
}
