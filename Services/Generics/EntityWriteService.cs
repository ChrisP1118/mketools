using EFCore.BulkExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Exceptions;
using MkeAlerts.Web.Models.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services
{
    public abstract class EntityWriteService<TDataModel, TIdType> : EntityReadService<TDataModel, TIdType>, IEntityWriteService<TDataModel, TIdType>
        where TDataModel : class, IHasId<TIdType>
    {
        //protected readonly ILogger<EntityWriteService<TDataModel, TIdType>> _logger;
        protected readonly IValidator<TDataModel> _validator;

        public EntityWriteService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<TDataModel> validator, ILogger<EntityWriteService<TDataModel, TIdType>> logger) : base(dbContext, userManager, logger)
        {
            //_logger = logger;
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

            await OnCreated(dataModel);

            return dataModel;
        }

        public async Task<Tuple<IEnumerable<TDataModel>, IEnumerable<TDataModel>>> BulkCreate(ClaimsPrincipal user, IList<TDataModel> dataModels, bool useBulkInsert = true)
        {
            var applicationUser = await GetApplicationUser(user);

            // Throw any errors first before adding these to our context
            foreach (TDataModel dataModel in dataModels)
            {
                if (!await CanCreate(applicationUser, dataModel))
                    throw new ForbiddenException();

                _validator.ValidateAndThrow(dataModel);
            }

            IList<TDataModel> success = new List<TDataModel>();
            IList<TDataModel> failure = new List<TDataModel>();

            if (useBulkInsert)
            {
                try
                {
                    await _dbContext.BulkInsertOrUpdateAsync<TDataModel>(dataModels, new BulkConfig
                    {
                        SqlBulkCopyOptions = SqlBulkCopyOptions.Default
                    });
                    success = dataModels;
                }
                catch (Exception ex)
                {
                    foreach (TDataModel dataModel in dataModels)
                    {
                        try
                        {
                            await _dbContext.BulkInsertOrUpdateAsync<TDataModel>(new List<TDataModel>() { dataModel });
                            success.Add(dataModel);
                        }
                        catch (Exception ex2)
                        {
                            _logger.LogError(ex, "Error bulk inserting item");
                            failure.Add(dataModel);
                        }
                    }
                }
            }
            else
            {
                _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

                foreach (TDataModel dataModel in dataModels)
                {
                    try
                    {
                        await _dbContext
                            .Set<TDataModel>()
                            .Upsert(dataModel)
                            .RunAsync();
                        success.Add(dataModel);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error upserting item");
                        failure.Add(dataModel);
                    }
                }
            }

            foreach (TDataModel dataModel in dataModels)
                await OnCreated(dataModel);

            return new Tuple<IEnumerable<TDataModel>, IEnumerable<TDataModel>>(success, failure);
        }

        public async Task<TDataModel> Update(ClaimsPrincipal user, TDataModel dataModel)
        {
            var applicationUser = await GetApplicationUser(user);

            if (!await CanUpdate(applicationUser, dataModel))
                throw new ForbiddenException();

            _validator.ValidateAndThrow(dataModel);

            _dbContext.Entry(dataModel).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            await OnUpdated(dataModel);

            return dataModel;
        }

        public async Task<TDataModel> Delete(ClaimsPrincipal user, TIdType id)
        {
            var applicationUser = await GetApplicationUser(user);

            TDataModel dataModel = await GetItemById(applicationUser, id, null);

            if (!await CanDelete(applicationUser, dataModel))
                throw new ForbiddenException();

            _dbContext.Set<TDataModel>().Remove(dataModel);
            await _dbContext.SaveChangesAsync();

            await OnDeleted(dataModel);

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

        protected virtual async Task OnCreated(TDataModel dataModel) { }
        protected virtual async Task OnUpdated(TDataModel dataModel) { }
        protected virtual async Task OnDeleted(TDataModel dataModel) { }
    }

}
