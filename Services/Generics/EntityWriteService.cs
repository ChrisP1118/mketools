using EFCore.BulkExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Data;
using MkeTools.Web.Exceptions;
using MkeTools.Web.Models.Data;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Data.Places;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeTools.Web.Services
{
    public abstract class EntityWriteService<TDataModel, TIdType> : EntityReadService<TDataModel, TIdType>, IEntityWriteService<TDataModel, TIdType>
        where TDataModel : class, IHasId<TIdType>
    {
        protected readonly IValidator<TDataModel> _validator;

        public EntityWriteService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<TDataModel> validator, ILogger<EntityWriteService<TDataModel, TIdType>> logger) : base(dbContext, userManager, logger)
        {
            _validator = validator;
        }

        #region CRUD Operations

        public async Task<TDataModel> Create(ClaimsPrincipal user, TDataModel dataModel)
        {
            var applicationUser = await GetApplicationUser(user);

            if (!await CanCreate(applicationUser, dataModel))
                throw new ForbiddenException();

            TruncateFields(dataModel);

            _validator.ValidateAndThrow(dataModel);

            _dbContext.Set<TDataModel>().Add(dataModel);
            await _dbContext.SaveChangesAsync();

            await OnCreated(dataModel);

            return dataModel;
        }

        public async Task<Tuple<IEnumerable<TDataModel>, IEnumerable<TDataModel>>> BulkUpsert(ClaimsPrincipal user, IList<TDataModel> dataModels, bool useBulkInsert = true)
        {
            var applicationUser = await GetApplicationUser(user);
            List<TDataModel> validatedDataModels = new List<TDataModel>();

            // Throw any errors first before adding these to our context
            foreach (TDataModel dataModel in dataModels)
            {
                if (!await CanCreate(applicationUser, dataModel))
                    throw new ForbiddenException();

                var validationResults = _validator.Validate(dataModel);
                if (validationResults.IsValid)
                    validatedDataModels.Add(dataModel);
                else
                    _logger.LogWarning("Validation failed for {Id}: {Errors}", dataModel.GetId(), string.Join("; ", validationResults.Errors.Select(x => x.ErrorMessage)));
            }

            // Remove duplicates (based on GetId)
            validatedDataModels = validatedDataModels.GroupBy(x => x.GetId()).Select(x => x.First()).ToList();

            List<TDataModel> success = new List<TDataModel>();
            List<TDataModel> failure = new List<TDataModel>();

            if (useBulkInsert)
            {
                try
                {
                    await _dbContext.BulkInsertOrUpdateAsync<TDataModel>(validatedDataModels, new BulkConfig
                    {
                        SqlBulkCopyOptions = Microsoft.Data.SqlClient.SqlBulkCopyOptions.Default
                    });
                    success = validatedDataModels;
                }
                catch (Exception ex)
                {
                    foreach (TDataModel dataModel in validatedDataModels)
                    {
                        try
                        {
                            await _dbContext.BulkInsertOrUpdateAsync<TDataModel>(new List<TDataModel>() { dataModel });
                            success.Add(dataModel);
                        }
                        catch (Exception ex2)
                        {
                            _logger.LogError(ex, "Error bulk inserting item {Id}", dataModel.GetId());
                            failure.Add(dataModel);
                        }
                    }
                }
            }
            else
            {
                _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

                try
                {
                    await _dbContext
                        .Set<TDataModel>()
                        .UpsertRange(validatedDataModels)
                        .RunAsync();

                    success.AddRange(validatedDataModels);
                }
                catch (Exception ex)
                {
                    foreach (TDataModel dataModel in validatedDataModels)
                    {
                        try
                        {
                            await _dbContext
                                .Set<TDataModel>()
                                .Upsert(dataModel)
                                .RunAsync();
                            success.Add(dataModel);
                        }
                        catch (Exception ex2)
                        {
                            _logger.LogError(ex2, "Error upserting item {Id}", dataModel.GetId());
                            failure.Add(dataModel);
                        }
                    }
                }

            }

            foreach (TDataModel dataModel in validatedDataModels)
                await OnCreated(dataModel);

            return new Tuple<IEnumerable<TDataModel>, IEnumerable<TDataModel>>(success, failure);
        }

        public async Task<TDataModel> Update(ClaimsPrincipal user, TDataModel dataModel)
        {
            var applicationUser = await GetApplicationUser(user);

            if (!await CanUpdate(applicationUser, dataModel))
                throw new ForbiddenException();

            TruncateFields(dataModel);

            _validator.ValidateAndThrow(dataModel);

            _dbContext.Entry(dataModel).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            await OnUpdated(dataModel);

            return dataModel;
        }

        public async Task<IEnumerable<TDataModel>> BulkUpdate(ClaimsPrincipal user, IEnumerable<TDataModel> dataModels)
        {
            var applicationUser = await GetApplicationUser(user);

            foreach (TDataModel dataModel in dataModels)
            {
                if (!await CanCreate(applicationUser, dataModel))
                    throw new ForbiddenException();

                _validator.ValidateAndThrow(dataModel);
            }

            foreach (TDataModel dataModel in dataModels)
                _dbContext.Entry(dataModel).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            foreach (TDataModel dataModel in dataModels) 
                await OnUpdated(dataModel);

            return dataModels;
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

        protected virtual bool DoTruncateFields { get { return true; } }

        private void TruncateFields(TDataModel dataModel)
        {
            if (!DoTruncateFields)
                return;

            var entityType = _dbContext.Model.FindEntityType(typeof(TDataModel));
            foreach (var property in entityType.GetProperties())
            {
                var annotation = property.GetAnnotations().FirstOrDefault(x => x.Name == "MaxLength");
                if (annotation != null)
                {
                    var maxLength = Convert.ToInt32(annotation.Value);
                    if (maxLength > 0)
                    {
                        var propertyInfo = dataModel.GetType().GetProperty(property.Name);
                        if (propertyInfo != null && propertyInfo.PropertyType == typeof(string))
                        {
                            string val = (string)propertyInfo.GetValue(dataModel);
                            if (val != null && val.Length > maxLength)
                            {
                                val = val.Substring(0, maxLength);
                                propertyInfo.SetValue(dataModel, val);
                            }
                        }
                    }
                }
            }
        }
    }

}
