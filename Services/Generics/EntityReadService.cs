using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Exceptions;
using MkeAlerts.Web.Models.Data;
using MkeAlerts.Web.Models.Data.Accounts;
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

        public EntityReadService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        #region CRUD Operations

        public async Task<List<TDataModel>> GetAll(ClaimsPrincipal user, int offset, int limit, string order, string filter)
        {
            var applicationUser = await GetApplicationUser(user);

            IQueryable<TDataModel> queryable = (await GetDataSet(applicationUser));

            if (!string.IsNullOrEmpty(filter))
                queryable = queryable.Where(GetParsingConfig(), filter);

            if (!string.IsNullOrEmpty(order))
                queryable = queryable.OrderBy(order);

            queryable = queryable
                .Skip(offset)
                .Take(limit);

            List<TDataModel> dataModelItems = await queryable.ToListAsync();

            return dataModelItems;
        }

        public async Task<long> GetAllCount(ClaimsPrincipal user, string filter)
        {
            var applicationUser = await GetApplicationUser(user);

            IQueryable<TDataModel> queryable = (await GetDataSet(applicationUser));

            if (!string.IsNullOrEmpty(filter))
                queryable = queryable.Where(GetParsingConfig(), filter);

            long count = await queryable.LongCountAsync();
            return count;
        }

        public async Task<TDataModel> GetOne(ClaimsPrincipal user, TIdType id)
        {
            var applicationUser = await GetApplicationUser(user);

            return await GetItemById(applicationUser, id);
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
        protected async Task<TDataModel> GetItemById(ApplicationUser applicationUser, TIdType id)
        {
            IQueryable<TDataModel> queryable = await GetDataSet(applicationUser);
            queryable = await ApplyIdFilter(queryable, id);
            return await queryable.SingleOrDefaultAsync();
        }

        protected abstract Task<IQueryable<TDataModel>> ApplyIdFilter(IQueryable<TDataModel> queryable, TIdType id);
    }
}
