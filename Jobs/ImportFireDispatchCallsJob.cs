using HtmlAgilityPack;
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
using Newtonsoft.Json.Linq;
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
    public class ImportFireDispatchCallsJob : LoggedJob
    {
        private readonly IEntityWriteService<FireDispatchCall, string> _fireDispatchCallWriteService;
        private readonly IGeocodingService _geocodingService;

        public ImportFireDispatchCallsJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<ImportFireDispatchCallsJob> logger, IEntityWriteService<FireDispatchCall, string> fireDispatchCallWriteService, IGeocodingService geocodingService)
            : base(configuration, signInManager, userManager, mailerService, jobRunService, logger)
        {
            _fireDispatchCallWriteService = fireDispatchCallWriteService;
            _geocodingService = geocodingService;
        }

        protected override async Task RunInternal()
        {
            ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

            string url = @"https://itmdapps.milwaukee.gov/MFDCallData/GetCalls";

            string data = null;
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();
                data = await response.Content.ReadAsStringAsync();
            }

            if (data != null)
            {
                JObject wrapper = JObject.Parse(data);
                //JArray items = JArray.Parse(data);
                foreach (JObject item in wrapper["result"].Children<JObject>())
                {
                    try
                    {
                        FireDispatchCall dispatchCall = await _fireDispatchCallWriteService.GetOne(claimsPrincipal, (string)item.Property("CFS").Value, null);

                        if (dispatchCall == null)
                        {
                            dispatchCall = new FireDispatchCall()
                            {
                                CFS = (string)item.Property("CFS").Value,
                                ReportedDateTime = DateTime.Parse((string)item.Property("DATEANDTIME").Value),
                                Address = (string)item.Property("ADDRESS").Value,
                                Apt = (string)item.Property("APT").Value,
                                City = (string)item.Property("CITY").Value,
                                NatureOfCall = (string)item.Property("ITYPE").Value,
                                Disposition = (string)item.Property("DISPOSITION").Value
                            };

                            GeocodeResults geocodeResults = await _geocodingService.Geocode(dispatchCall.Address);
                            dispatchCall.Geometry = geocodeResults.Geometry;
                            dispatchCall.Accuracy = geocodeResults.Accuracy;
                            dispatchCall.Source = geocodeResults.Source;
                            dispatchCall.LastGeocodeAttempt = DateTime.Now;

                            GeographicUtilities.SetBounds(dispatchCall, geocodeResults.Geometry);

                            await _fireDispatchCallWriteService.Create(claimsPrincipal, dispatchCall);
                            ++_successCount;
                        }
                        else
                        {
                            if (item.Property("DISPOSITION") != null)
                                dispatchCall.Disposition = (string)item.Property("DISPOSITION").Value;
                            if (item.Property("disposition") != null)
                                dispatchCall.Disposition = (string)item.Property("disposition").Value;

                            await _fireDispatchCallWriteService.Update(claimsPrincipal, dispatchCall);
                            ++_successCount;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error importing FireDispatchCall");
                        ++_failureCount;
                    }
                }
            }
        }
    }
}