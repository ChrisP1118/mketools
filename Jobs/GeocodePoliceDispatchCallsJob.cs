using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Incidents;
using MkeAlerts.Web.Models.Internal;
using MkeAlerts.Web.Services;
using MkeAlerts.Web.Services.Data.Interfaces;
using MkeAlerts.Web.Services.Functional;
using MkeAlerts.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Jobs
{
    public class GeocodePoliceDispatchCallsJob : GeocodeItemsJob<PoliceDispatchCall, string>
    {
        public GeocodePoliceDispatchCallsJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<GeocodePoliceDispatchCallsJob> logger, IEntityWriteService<PoliceDispatchCall, string> writeService, IGeocodingService geocodingService)
            : base(configuration, signInManager, userManager, mailerService, jobRunService, logger, writeService, geocodingService)
        {

        }

        protected override IQueryable<PoliceDispatchCall> GetFilter(IQueryable<PoliceDispatchCall> queryable)
        {
            return queryable
                .Where(x => x.Geometry == null);
        }

        protected override string GetGeocodeValue(PoliceDispatchCall dataModel)
        {
            return dataModel.Location;
        }

        protected override void SetGeocodeResults(PoliceDispatchCall dataModel, GeocodeResults geocodeResults)
        {
            dataModel.Geometry = geocodeResults.Geometry;
        }
    }
}
