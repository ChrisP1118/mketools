using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Models.Data;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Internal;
using MkeTools.Web.Services;
using MkeTools.Web.Services.Data.Interfaces;
using MkeTools.Web.Services.Functional;
using MkeTools.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeTools.Web.Jobs
{
    public abstract class GeocodeItemsJob<TDataModel, TIdType> : LoggedJob
        where TDataModel : class, IHasId<TIdType>, IGeocodable, new()
    {
        private readonly IEntityWriteService<TDataModel, TIdType> _writeService;
        private readonly IGeocodingService _geocodingService;
        private const int _batchSize = 100;

        protected string _shapefileName = null;

        public GeocodeItemsJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<GeocodeItemsJob<TDataModel, TIdType>> logger, IEntityWriteService<TDataModel, TIdType> writeService, IGeocodingService geocodingService)
            : base(configuration, signInManager, userManager, mailerService, jobRunService, logger)
        {
            _writeService = writeService;
            _geocodingService = geocodingService;
        }

        protected abstract IQueryable<TDataModel> GetFilter(IQueryable<TDataModel> queryable);
        protected abstract string GetGeocodeValue(TDataModel dataModel);
        protected abstract void SetGeocodeResults(TDataModel dataModel, GeocodeResults geocodeResults);

        protected override async Task RunInternal()
        {
            ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

            do
            {
                List<TDataModel> dataModels = await _writeService.GetAll(claimsPrincipal, 0, _batchSize, null, null, null, null, null, null, null, false, false, queryable => GetFilter(queryable).Where(x => x.LastGeocodeAttempt == null));

                if (dataModels.Count == 0)
                    break;

                foreach (TDataModel dataModel in dataModels)
                {
                    string value = GetGeocodeValue(dataModel);
                    GeocodeResults geocodeResults = await _geocodingService.Geocode(value);
                    SetGeocodeResults(dataModel, geocodeResults);

                    dataModel.Accuracy = geocodeResults.Accuracy;
                    dataModel.Source = geocodeResults.Source;
                    dataModel.LastGeocodeAttempt = DateTime.Now;

                    GeographicUtilities.SetBounds(dataModel, geocodeResults.Geometry);

                    if (geocodeResults.Accuracy == Models.GeometryAccuracy.NoGeometry)
                        ++_failureCount;
                    else
                        ++_successCount;
                }

                try
                {
                    await _writeService.BulkUpdate(claimsPrincipal, dataModels);
                    dataModels = new List<TDataModel>();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error bulk updating");
                }
            } while (true);
        }
    }
}
