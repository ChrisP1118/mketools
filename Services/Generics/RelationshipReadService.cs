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
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services
{
    public abstract class RelationshipReadService<TDataModel> : IRelationshipReadService<TDataModel>
        where TDataModel : class
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

        public RelationshipReadService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        protected abstract string ParentIdPropertyName { get; }
        protected abstract string ChildIdPropertyName { get; }

        protected abstract string ParentPropertyName { get; }
        protected abstract string ChildPropertyName { get; }

        /// <summary>
        /// Applies parent filtering to a queryable. Generally, this would return the queryable that's passed in, but with a Where clause that limits results by parentId
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        protected virtual IQueryable<TDataModel> FilterByParentId(IQueryable<TDataModel> queryable, Guid parentId)
        {
            return queryable.Where(ParentIdPropertyName + " == @0", parentId);
        }

        /// <summary>
        /// Applies child filtering to a queryable. Generally, this would return the queryable that's passed in, but with a Where clause that limits results by childId
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="childId"></param>
        /// <returns></returns>
        protected virtual IQueryable<TDataModel> FilterByChildId(IQueryable<TDataModel> queryable, Guid childId)
        {
            return queryable.Where(ChildIdPropertyName + " == @0", childId);
        }

        /// <summary>
        /// Applies EF eager loading to a queryable
        /// </summary>
        /// <param name="queryable"></param>
        /// <returns></returns>
        protected virtual IQueryable<TDataModel> IncludeChildren(IQueryable<TDataModel> queryable)
        {
            return queryable.Include(ChildPropertyName);
        }

        #region CRUD Operations

        public async Task<List<TDataModel>> GetAll(ClaimsPrincipal user, Guid parentId, int offset, int limit, string order, string filter)
        {
            var applicationUser = await GetApplicationUser(user);

            IQueryable<TDataModel> queryable = (await GetDataSet(applicationUser, parentId));

            queryable = IncludeChildren(queryable);

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

        public async Task<long> GetAllCount(ClaimsPrincipal user, Guid parentId, string filter)
        {
            var applicationUser = await GetApplicationUser(user);

            IQueryable<TDataModel> queryable = (await GetDataSet(applicationUser, parentId));

            if (!string.IsNullOrEmpty(filter))
                queryable = queryable.Where(GetParsingConfig(), filter);

            long count = await queryable.LongCountAsync();
            return count;
        }

        public async Task<TDataModel> GetOne(ClaimsPrincipal user, Guid parentId, Guid id)
        {
            var applicationUser = await GetApplicationUser(user);

            return await GetItemById(applicationUser, parentId, id);
        }

        #endregion

        #region Basic Operation Permissions

        /// <summary>
        /// Returns a queryable with any security-related read filtering applied. This is designed to be overwritten in derived classes to enforce their own security model
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="parentId"></param>
        /// <param name="queryable"></param>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TDataModel>> ApplyReadSecurity(ApplicationUser applicationUser, Guid parentId, IQueryable<TDataModel> queryable) { return queryable; }

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
        /// <param name="parentId"></param>
        /// <returns></returns>
        protected async Task<IQueryable<TDataModel>> GetDataSet(ApplicationUser applicationUser, Guid parentId)
        {
            IQueryable<TDataModel> queryable = _dbContext.Set<TDataModel>();
            queryable = FilterByParentId(queryable, parentId);
            return await ApplyReadSecurity(applicationUser, parentId, queryable);
        }

        /// <summary>
        /// Gets an item by ID, ensuring the user has access to it
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="parentId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected async Task<TDataModel> GetItemById(ApplicationUser applicationUser, Guid parentId, Guid id)
        {
            IQueryable<TDataModel> queryable = await GetDataSet(applicationUser, parentId);
            queryable = FilterByChildId(queryable, id);
            queryable = IncludeChildren(queryable);
            return await queryable.SingleOrDefaultAsync();
        }

        /// <summary>
        /// Gets the parent ID value on an existing model
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public Guid GetParentId(TDataModel dataModel)
        {
            PropertyInfo propertyInfo = dataModel.GetType().GetProperty(ParentIdPropertyName);
            return (Guid)propertyInfo.GetValue(dataModel);
        }

        /// <summary>
        /// Gets the child ID value on an existing model
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public Guid GetChildId(TDataModel dataModel)
        {
            PropertyInfo propertyInfo = dataModel.GetType().GetProperty(ChildIdPropertyName);
            return (Guid)propertyInfo.GetValue(dataModel);
        }
    }
}
