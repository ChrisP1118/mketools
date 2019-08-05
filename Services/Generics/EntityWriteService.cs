using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Exceptions;
using MkeAlerts.Web.Models.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services
{
    public abstract class EntityWriteService<TDataModel, TIdType> : EntityReadService<TDataModel, TIdType>, IEntityWriteService<TDataModel, TIdType>
        where TDataModel : class, IHasId<TIdType>
    {
        protected readonly IValidator<TDataModel> _validator;

        public EntityWriteService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<TDataModel> validator) : base(dbContext, userManager)
        {
            _validator = validator;
        }

        #region CRUD Operations

        public async Task<TDataModel> Create(ClaimsPrincipal user, TDataModel dataModel)
        {
            var applicationUser = await GetApplicationUser(user);

            if (!await CanCreate(applicationUser, dataModel))
                throw new ForbiddenException();

            _validator.ValidateAndThrow(dataModel);

            _dbContext.Set<TDataModel>().Add(dataModel);
            await _dbContext.SaveChangesAsync();

            return dataModel;
        }

        public async Task<IEnumerable<TDataModel>> Create(ClaimsPrincipal user, IEnumerable<TDataModel> dataModels)
        {
            var applicationUser = await GetApplicationUser(user);

            foreach (TDataModel dataModel in dataModels)
            {

                if (!await CanCreate(applicationUser, dataModel))
                    throw new ForbiddenException();

                _validator.ValidateAndThrow(dataModel);

                _dbContext.Set<TDataModel>().Add(dataModel);
            }

            await _dbContext.SaveChangesAsync();

            return dataModels;
        }

        public async Task<TDataModel> Update(ClaimsPrincipal user, TDataModel dataModel)
        {
            var applicationUser = await GetApplicationUser(user);

            if (!await CanUpdate(applicationUser, dataModel))
                throw new ForbiddenException();

            _validator.ValidateAndThrow(dataModel);

            _dbContext.Entry(dataModel).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return dataModel;
        }

        public async Task<TDataModel> Delete(ClaimsPrincipal user, TIdType id)
        {
            var applicationUser = await GetApplicationUser(user);

            TDataModel dataModel = await GetItemById(applicationUser, id);

            if (!await CanDelete(applicationUser, dataModel))
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
        protected virtual async Task<bool> CanWrite(ApplicationUser applicationUser, TDataModel dataModel) { return true; }
        protected virtual async Task<bool> CanCreate(ApplicationUser applicationUser, TDataModel dataModel) { return await CanWrite(applicationUser, dataModel); }
        protected virtual async Task<bool> CanUpdate(ApplicationUser applicationUser, TDataModel dataModel) { return await CanWrite(applicationUser, dataModel); }
        protected virtual async Task<bool> CanDelete(ApplicationUser applicationUser, TDataModel dataModel) { return await CanWrite(applicationUser, dataModel); }

        #endregion

    }

}
