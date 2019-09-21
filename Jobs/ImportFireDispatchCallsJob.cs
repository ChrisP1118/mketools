using GeoAPI.Geometries;
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
    public class ImportFireDispatchCallsJob : Job
    {
        private readonly ILogger<ImportFireDispatchCallsJob> _logger;
        private readonly IEntityWriteService<FireDispatchCall, string> _fireDispatchCallWriteService;
        private readonly IGeocodingService _geocodingService;

        public ImportFireDispatchCallsJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<ImportFireDispatchCallsJob> logger, IEntityWriteService<FireDispatchCall, string> fireDispatchCallWriteService, IGeocodingService geocodingService)
            : base(configuration, signInManager, userManager)
        {
            _fireDispatchCallWriteService = fireDispatchCallWriteService;
            _logger = logger;
            _geocodingService = geocodingService;
        }

        public async Task Run()
        {
            _logger.LogInformation("Starting job");

            ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

            string url = @"https://itmdapps.milwaukee.gov/MilRest/mfd/calls";

            string data = null;
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();
                data = await response.Content.ReadAsStringAsync();
            }

            int success = 0;
            int failure = 0;

            if (data != null)
            {
                JArray items = JArray.Parse(data);
                foreach (JObject item in items.Children<JObject>())
                {
                    try
                    {
                        FireDispatchCall fireDispatchCall = await _fireDispatchCallWriteService.GetOne(claimsPrincipal, (string)item.Property("cfs").Value);

                        if (fireDispatchCall == null)
                        {
                            fireDispatchCall = new FireDispatchCall()
                            {
                                CFS = (string)item.Property("cfs").Value,
                                ReportedDateTime = DateTime.Parse((string)item.Property("callDate").Value),
                                Address = (string)item.Property("address").Value,
                                Apt = (string)item.Property("apt").Value,
                                City = (string)item.Property("city").Value,
                                NatureOfCall = (string)item.Property("type").Value,
                                Disposition = (string)item.Property("disposition").Value
                            };

                            GeocodeResults geocodeResults = await _geocodingService.Geocode(fireDispatchCall.Address);
                            fireDispatchCall.Geometry = geocodeResults.Geometry;
                            fireDispatchCall.Accuracy = geocodeResults.Accuracy;
                            fireDispatchCall.Source = geocodeResults.Source;

                            GeographicUtilities.SetBounds(fireDispatchCall, geocodeResults.Geometry);

                            await _fireDispatchCallWriteService.Create(claimsPrincipal, fireDispatchCall);
                            ++success;
                        }
                        else
                        {
                            fireDispatchCall.Disposition = (string)item.Property("disposition").Value;

                            await _fireDispatchCallWriteService.Update(claimsPrincipal, fireDispatchCall);
                            ++success;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error importing FireDispatchCall");
                        ++failure;
                    }
                }
            }

            _logger.LogInformation("Import results: " + success.ToString() + " success, " + failure.ToString() + " failure");
            _logger.LogInformation("Finishing job");
        }
    }
}