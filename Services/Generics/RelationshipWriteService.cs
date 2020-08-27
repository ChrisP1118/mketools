using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MkeTools.Web.Data;
using MkeTools.Web.Exceptions;
using MkeTools.Web.Models.Data;
using MkeTools.Web.Models.Data.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeTools.Web.Services
{
    public abstract class RelationshipWriteService<TDataModel> : RelationshipReadService<TDataModel>, IRelationshipWriteService<TDataModel>
        where TDataModel : class
    {
        protected readonly IValidator<TDataModel> _validator;

        public RelationshipWriteService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<TDataModel> validator) : base(dbContext, userManager)
        {
            _validator = validator;
        }

        protected virtual void SetParentId(TDataModel dataModel, Guid parentId)
        {
            PropertyInfo propertyInfo = dataModel.GetType().GetProperty(ParentIdPropertyName);
            propertyInfo.SetValue(dataModel, parentId);
        }

        #region CRUD Operations

        public async Task<TDataModel> Create(ClaimsPrincipal user, Guid parentId, TDataModel dataModel)
        {
            var applicationUser = await GetApplicationUser(user);

            if (!await CanCreate(applicationUser, parentId, dataModel))
                throw new ForbiddenException();

            SetParentId(dataModel, parentId);

            _validator.ValidateAndThrow(dataModel);

            _dbContext.Set<TDataModel>().Add(dataModel);
            await _dbContext.SaveChangesAsync();

            return dataModel;
        }

        public async Task<TDataModel> Update(ClaimsPrincipal user, Guid parentId, TDataModel dataModel)
        {
            var applicationUser = await GetApplicationUser(user);

            if (!await CanUpdate(applicationUser, parentId, dataModel))
                throw new ForbiddenException();

            _validator.ValidateAndThrow(dataModel);

            _dbContext.Entry(dataModel).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return dataModel;
        }

        public async Task<TDataModel> Delete(ClaimsPrincipal user, Guid parentId, Guid id)
        {
            var applicationUser = await GetApplicationUser(user);

            TDataModel dataModel = await GetItemById(applicationUser, parentId, id);

            if (!await CanDelete(applicationUser, parentId, dataModel))
                throw new ForbiddenException();

            _dbContext.Set<TDataModel>().Remove(dataModel);
            await _dbContext.SaveChangesAsync();

            return dataModel;
        }

        #endregion

        #region Basic Operation Permissions

        /// <summary>
        /// Returns whether or not a user can perform non-read (create, update, delete) operations on a model. By default, CanCreate, CanUpdate, and CanDelete call this method, but each of them
        /// may provide their own custom implementation that won't call this method. The data model passed in should already contain all the data needed to make a determination on the user's
        /// ability to write the data.
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        protected virtual async Task<bool> CanWrite(ApplicationUser applicationUser, Guid parentId, TDataModel dataModel) { return true; }
        protected virtual async Task<bool> CanCreate(ApplicationUser applicationUser, Guid parentId, TDataModel dataModel) { return await CanWrite(applicationUser, parentId, dataModel); }
        protected virtual async Task<bool> CanUpdate(ApplicationUser applicationUser, Guid parentId, TDataModel dataModel) { return await CanWrite(applicationUser, parentId, dataModel); }
        protected virtual async Task<bool> CanDelete(ApplicationUser applicationUser, Guid parentId, TDataModel dataModel) { return await CanWrite(applicationUser, parentId, dataModel); }

        #endregion
    }

}
