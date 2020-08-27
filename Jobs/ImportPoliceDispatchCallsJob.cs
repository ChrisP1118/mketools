using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Data.Incidents;
using MkeTools.Web.Models.Internal;
using MkeTools.Web.Services;
using MkeTools.Web.Services.Data.Interfaces;
using MkeTools.Web.Services.Functional;
using MkeTools.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeTools.Web.Jobs
{
    public class ImportPoliceDispatchCallsJob : LoggedJob
    {
        private readonly IEntityWriteService<PoliceDispatchCall, string> _policeDispatchCallWriteService;
        private readonly IGeocodingService _geocodingService;

        public ImportPoliceDispatchCallsJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<ImportPoliceDispatchCallsJob> logger, IEntityWriteService<PoliceDispatchCall, string> policeDispatchCallWriteService, IGeocodingService geocodingService)
            : base(configuration, signInManager, userManager, mailerService, jobRunService, logger)
        {
            _policeDispatchCallWriteService = policeDispatchCallWriteService;
            _geocodingService = geocodingService;
        }

        protected override async Task RunInternal()
        {
            ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

            string url = @"https://itmdapps.milwaukee.gov/MPDCallData/index.jsp?district=All";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            foreach (var row in doc.DocumentNode.SelectNodes("//table/tbody/tr"))
            {
                try
                {
                    var cols = row.SelectNodes("td");

                    PoliceDispatchCall dispatchCall = await _policeDispatchCallWriteService.GetOne(claimsPrincipal, cols[0].InnerText, null);

                    if (dispatchCall == null)
                    {
                        dispatchCall = new PoliceDispatchCall()
                        {
                            CallNumber = cols[0].InnerText,
                            ReportedDateTime = DateTime.Parse(cols[1].InnerText),
                            Location = cols[2].InnerText,
                            District = ParsingUtilities.ParseInt(cols[3].InnerText, 0),
                            NatureOfCall = cols[4].InnerText,
                            Status = cols[5].InnerText
                        };

                        GeocodeResults geocodeResults = await _geocodingService.Geocode(dispatchCall.Location);
                        dispatchCall.Geometry = geocodeResults.Geometry;
                        dispatchCall.Accuracy = geocodeResults.Accuracy;
                        dispatchCall.Source = geocodeResults.Source;
                        dispatchCall.LastGeocodeAttempt = DateTime.Now;

                        GeographicUtilities.SetBounds(dispatchCall, geocodeResults.Geometry);

                        await _policeDispatchCallWriteService.Create(claimsPrincipal, dispatchCall);
                        ++_successCount;
                    }
                    else
                    {
                        dispatchCall.Status = cols[5].InnerText;

                        await _policeDispatchCallWriteService.Update(claimsPrincipal, dispatchCall);
                        ++_successCount;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error importing PoliceDispatchCall");
                    ++_failureCount;
                }
            }
        }
    }
}