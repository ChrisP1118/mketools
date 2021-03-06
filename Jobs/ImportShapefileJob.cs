﻿using DotSpatial.Projections;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Models.Data;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Services;
using MkeTools.Web.Services.Data;
using MkeTools.Web.Services.Data.Interfaces;
using MkeTools.Web.Services.Functional;
using MkeTools.Web.Utilities;
using NetTopologySuite;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.IO.ShapeFile.Extended;
using NetTopologySuite.IO.ShapeFile.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Parcel = MkeTools.Web.Models.Data.Places.Parcel;

namespace MkeTools.Web.Jobs
{
    public abstract class ImportShapefileJob<TDataModel, TIdType> : LoggedJob
        where TDataModel : class, IHasId<TIdType>, new()
    {
        private readonly IEntityWriteService<TDataModel, TIdType> _writeService;
        private const int _batchSize = 100;

        protected string _shapefileName = null;

        public ImportShapefileJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<ImportShapefileJob<TDataModel, TIdType>> logger, IEntityWriteService<TDataModel, TIdType> writeService)
            : base(configuration, signInManager, userManager, mailerService, jobRunService, logger)
        {
            _writeService = writeService;
        }

        protected abstract bool VerifyItem(IShapefileFeature source, TDataModel target);

        protected override async Task RunInternal()
        {
            string shapefilePath = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            string fullPath = shapefilePath + "\\" + _shapefileName;

            using (ShapeDataReader reader = new ShapeDataReader(_shapefileName))
            {
                var mbr = reader.ShapefileBounds;
                var result = reader.ReadByMBRFilter(mbr);
                var coll = result.GetEnumerator();

                List<TDataModel> dataModels = new List<TDataModel>();

                int i = 0;
                while (coll.MoveNext())
                {
                    ++i;

                    TDataModel dataModel = new TDataModel();

                    try
                    {
                        if (!ShapefileUtilities.CopyFields(coll.Current, dataModel, i, _logger))
                        {
                            _logger.LogDebug("Skipping record {Index}: {ID} - CopyFields returned false", i, dataModel.GetId());
                            ++_failureCount;
                            continue;
                        }

                        if (!VerifyItem(coll.Current, dataModel))
                        {
                            _logger.LogDebug("Skipping record {Index}: {ID} - VerifyItem returned false", i, dataModel.GetId());
                            ++_failureCount;
                            continue;
                        }

                        dataModels.Add(dataModel);

                        if (i % _batchSize == 0)
                            await MergeItems(dataModels, i);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error importing item {Index}: {Id}", i, dataModel.GetId());
                    }
                }

                await MergeItems(dataModels, i);
            }
        }

        private async Task MergeItems(List<TDataModel> dataModels, int i)
        {
            ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

            try
            {
                Tuple<IEnumerable<TDataModel>, IEnumerable<TDataModel>> results = await _writeService.BulkUpsert(claimsPrincipal, dataModels, false);
                _successCount += results.Item1.Count();
                _failureCount += results.Item2.Count();
                dataModels.Clear();

                _logger.LogDebug("Bulk inserted items at mod {Index}", i);
            }
            catch (Exception ex)
            {
                _failureCount += dataModels.Count;

                _logger.LogError(ex, "Error bulk inserting items at mod {Index}", i);
            }

        }
    }
}
