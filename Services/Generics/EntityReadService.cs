using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Exceptions;
using MkeAlerts.Web.Models.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services
{
    public abstract class EntityReadService<TDataModel, TIdType> : IEntityReadService<TDataModel, TIdType>
        where TDataModel : class, IHasId<TIdType>
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly ILogger<EntityReadService<TDataModel, TIdType>> _logger;

        protected static ParsingConfig _parsingConfig;
        protected static ParsingConfig GetParsingConfig()
        {
            if (_parsingConfig == null)
            {
                _parsingConfig = new ParsingConfig();
                _parsingConfig.CustomTypeProvider = new CustomDynamicLinqProvider();
            }

            return _parsingConfig;
        }

        public EntityReadService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, ILogger<EntityReadService<TDataModel, TIdType>> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
        }

        #region CRUD Operations

        public async Task<List<TDataModel>> GetAll(ClaimsPrincipal user, int offset, int limit, string order, string includes, string filter, double? northBound, double? southBound, double? eastBound, double? westBound, bool useHighPrecision = true, bool noTracking = false, Func<IQueryable<TDataModel>, IQueryable<TDataModel>> filterFunc = null)
        {
            var applicationUser = await GetApplicationUser(user);

            IQueryable<TDataModel> queryable = (await GetDataSet(applicationUser));

            queryable = await ApplyIncludes(queryable, includes);

            if (!string.IsNullOrEmpty(filter))
                queryable = queryable.Where(GetParsingConfig(), filter);

            if (filterFunc != null)
                queryable = filterFunc(queryable);

            if (northBound.HasValue && southBound.HasValue && eastBound.HasValue && westBound.HasValue)
                queryable = await ApplyBounds(queryable, northBound.Value, southBound.Value, eastBound.Value, westBound.Value, useHighPrecision);

            if (!string.IsNullOrEmpty(order))
                queryable = queryable.OrderBy(order);

            queryable = queryable
                .Skip(offset)
                .Take(limit);

            if (noTracking)
                queryable = queryable.AsNoTracking();

            List<TDataModel> dataModelItems = await queryable.ToListAsync();

            return dataModelItems;
        }

        public async Task<long> GetAllCount(ClaimsPrincipal user, string filter, double? northBound, double? southBound, double? eastBound, double? westBound, bool useHighPrecision = true)
        {
            var applicationUser = await GetApplicationUser(user);

            IQueryable<TDataModel> queryable = (await GetDataSet(applicationUser));

            if (!string.IsNullOrEmpty(filter))
                queryable = queryable.Where(GetParsingConfig(), filter);

            if (northBound.HasValue && southBound.HasValue && eastBound.HasValue && westBound.HasValue)
                queryable = await ApplyBounds(queryable, northBound.Value, southBound.Value, eastBound.Value, westBound.Value, useHighPrecision);

            long count = await queryable.LongCountAsync();
            return count;
        }

        public async Task<TDataModel> GetOne(ClaimsPrincipal user, TIdType id, string includes)
        {
            var applicationUser = await GetApplicationUser(user);

            return await GetItemById(applicationUser, id, includes);
        }

        #endregion

        #region Basic Operation Permissions

        /// <summary>
        /// Returns a queryable with any security-related read filtering applied. This is designed to be overwritten in derived classes to enforce their own security model
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="queryable"></param>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TDataModel>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<TDataModel> queryable) { return queryable; }

        #endregion

        /// <summary>
        /// Returns an ApplicationUser, with necessary security attributes, for a ClaimsPrincipal
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected async Task<ApplicationUser> GetApplicationUser(ClaimsPrincipal user)
        {
            if (user == null)
                return null;

            string userIdString = _userManager.GetUserId(user);
            if (string.IsNullOrEmpty(userIdString))
                return null;

            Guid userId;
            if (!Guid.TryParse(userIdString, out userId))
                return null;

            return await _dbContext.ApplicationUsers
                .Where(au => au.Id == userId)
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Gets the underlying dataset for the data model with read security applied
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TDataModel>> GetDataSet(ApplicationUser applicationUser)
        {
            IQueryable<TDataModel> queryable = _dbContext.Set<TDataModel>();
            return await ApplyReadSecurity(applicationUser, queryable);
        }

        /// <summary>
        /// Gets an item by ID, ensuring the user has access to it
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected async Task<TDataModel> GetItemById(ApplicationUser applicationUser, TIdType id, string includes)
        {
            IQueryable<TDataModel> queryable = await GetDataSet(applicationUser);
            queryable = await ApplyIncludes(queryable, includes);
            queryable = await ApplyIdFilter(queryable, id);
            return await queryable.SingleOrDefaultAsync();
        }

        protected abstract Task<IQueryable<TDataModel>> ApplyIdFilter(IQueryable<TDataModel> queryable, TIdType id);

        protected async Task<IQueryable<TDataModel>> ApplyBounds(IQueryable<TDataModel> queryable, double northBound, double southBound, double eastBound, double westBound, bool useHighPrecision)
        {
            Polygon bounds = new Polygon(new LinearRing(new Coordinate[]
            {
                new Coordinate(westBound, northBound), // NW
                new Coordinate(westBound, southBound), // SW
                new Coordinate(eastBound, southBound), // SE
                new Coordinate(eastBound, northBound), // NE
                new Coordinate(westBound, northBound), // NW
            }));
            bounds.SRID = 4326;

            return await ApplyBounds(queryable, northBound, southBound, eastBound, westBound, bounds, useHighPrecision);
        }

        protected virtual async Task<IQueryable<TDataModel>> ApplyBounds(IQueryable<TDataModel> queryable, double northBound, double southBound, double eastBound, double westBound, Polygon bounds, bool useHighPrecision)
        {
            return queryable;
        }

        protected virtual async Task<IQueryable<TDataModel>> ApplyIncludes(IQueryable<TDataModel> queryable, string includes)
        {
            if (string.IsNullOrEmpty(includes))
                return queryable;

            foreach (string include in includes.Split(","))
            {
                // Break this apart at the dots, and ensure that each character after a dot is upper case -- this converts it from camelCase to PascalCase. There's probably a regex that could do this more efficiently. But ugh, regex.
                string[] parts = include.Split(".");
                string pascalCasedInclude = string.Join(".", parts.Select(x => x.Substring(0, 1).ToUpper() + x.Substring(1)));
                queryable = queryable.Include(pascalCasedInclude);
            }

            return queryable;
        }
    }
}
