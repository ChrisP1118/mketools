using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Models.Internal;
using MkeAlerts.Web.Services.Data.Interfaces;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Data
{
    public class PropertyService : EntityWriteService<Property, Guid>, IPropertyService
    {
        public PropertyService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<Property> validator, ILogger<EntityWriteService<Property, Guid>> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<Property>> ApplyIdFilter(IQueryable<Property> queryable, Guid id)
        {
            return queryable.Where(x => x.Id == id);
        }

        protected override async Task<IQueryable<Property>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<Property> queryable)
        {
            return queryable;
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, Property dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }

        public async Task<Dictionary<string, CurrentPropertyRecord>> GetCurrentRecords(ClaimsPrincipal user)
        {
            var applicationUser = await GetApplicationUser(user);

            Dictionary<string, CurrentPropertyRecord> currentRecords = await _dbContext.CurrentPropertyRecords
                .FromSqlRaw(@"
                    select taxkey as TAXKEY, max(last_value_chg) as LAST_VALUE_CHG, max(last_name_chg) as LAST_NAME_CHG
                    from Properties
                    where Source = 'CurrentMprop'
                    group by taxkey")
                .AsNoTracking()
                .ToDictionaryAsync(x => x.TAXKEY, x => new CurrentPropertyRecord()
                {
                    TAXKEY = x.TAXKEY,
                    LAST_NAME_CHG = x.LAST_NAME_CHG,
                    LAST_VALUE_CHG = x.LAST_VALUE_CHG
                });

            return currentRecords;
        }

    }
}
