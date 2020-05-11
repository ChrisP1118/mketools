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
    public class GeocodeFireDispatchCallsJob : GeocodeItemsJob<FireDispatchCall, string>
    {
        public GeocodeFireDispatchCallsJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<GeocodeFireDispatchCallsJob> logger, IEntityWriteService<FireDispatchCall, string> writeService, IGeocodingService geocodingService)
            : base(configuration, signInManager, userManager, mailerService, jobRunService, logger, writeService, geocodingService)
        {

        }

        protected override IQueryable<FireDispatchCall> GetFilter(IQueryable<FireDispatchCall> queryable)
        {
            return queryable
                .Where(x => x.Geometry == null);
        }

        protected override string GetGeocodeValue(FireDispatchCall dataModel)
        {
            return dataModel.Address;
        }

        protected override void SetGeocodeResults(FireDispatchCall dataModel, GeocodeResults geocodeResults)
        {
            dataModel.Geometry = geocodeResults.Geometry;
        }
    }
}
