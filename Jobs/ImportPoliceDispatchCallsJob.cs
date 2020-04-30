using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Incidents;
using MkeAlerts.Web.Models.Internal;
using MkeAlerts.Web.Services;
using MkeAlerts.Web.Services.Functional;
using MkeAlerts.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Jobs
{
    public class ImportPoliceDispatchCallsJob : Job
    {
        private readonly ILogger<ImportPoliceDispatchCallsJob> _logger;
        private readonly IEntityWriteService<PoliceDispatchCall, string> _policeDispatchCallWriteService;
        private readonly IGeocodingService _geocodingService;

        public ImportPoliceDispatchCallsJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, ILogger<ImportPoliceDispatchCallsJob> logger, IEntityWriteService<PoliceDispatchCall, string> policeDispatchCallWriteService, IGeocodingService geocodingService)
            : base(configuration, signInManager, userManager, mailerService)
        {
            _policeDispatchCallWriteService = policeDispatchCallWriteService;
            _logger = logger;
            _geocodingService = geocodingService;
        }

        public async Task Run()
        {
            try
            {
                _logger.LogInformation("Starting job");

                ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

                string url = @"https://itmdapps.milwaukee.gov/MPDCallData/index.jsp?district=All";
                var web = new HtmlWeb();
                var doc = web.Load(url);

                int success = 0;
                int failure = 0;

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
                                District = int.Parse(cols[3].InnerText),
                                NatureOfCall = cols[4].InnerText,
                                Status = cols[5].InnerText
                            };

                            GeocodeResults geocodeResults = await _geocodingService.Geocode(dispatchCall.Location);
                            dispatchCall.Geometry = geocodeResults.Geometry;
                            dispatchCall.Accuracy = geocodeResults.Accuracy;
                            dispatchCall.Source = geocodeResults.Source;

                            GeographicUtilities.SetBounds(dispatchCall, geocodeResults.Geometry);

                            await _policeDispatchCallWriteService.Create(claimsPrincipal, dispatchCall);
                            ++success;
                        }
                        else
                        {
                            dispatchCall.Status = cols[5].InnerText;

                            await _policeDispatchCallWriteService.Update(claimsPrincipal, dispatchCall);
                            ++success;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error importing DispatchCall");
                        ++failure;
                    }
                }

                _logger.LogInformation("Import results: " + success.ToString() + " success, " + failure.ToString() + " failure");
                _logger.LogInformation("Finishing job");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error running " + this.GetType().Name + ": " + ex);

                await _mailerService.SendAdminAlert("Error running " + this.GetType().Name, ex.Message);
            }
        }
    }
}