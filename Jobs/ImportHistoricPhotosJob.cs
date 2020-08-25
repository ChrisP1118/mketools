using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.AppHealth;
using MkeAlerts.Web.Models.Data.HistoricPhotos;
using MkeAlerts.Web.Models.Internal;
using MkeAlerts.Web.Services;
using MkeAlerts.Web.Services.Data.Interfaces;
using MkeAlerts.Web.Services.Functional;
using MkeAlerts.Web.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Jobs
{
    public class ImportHistoricPhotosJob : LoggedJob
    {
        private static HttpClient _httpClient = new HttpClient();

        private readonly IGeocodingService _geocodingService;
        private readonly IEntityWriteService<HistoricPhoto, string> _historicPhotoService;
        private readonly IEntityWriteService<HistoricPhotoLocation, Guid> _historicPhotoLocationService;

        public ImportHistoricPhotosJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<LoggedJob> logger, IGeocodingService geocodingService, IEntityWriteService<HistoricPhoto, string> historicPhotoService, IEntityWriteService<HistoricPhotoLocation, Guid> historicPhotoLocationService) :
            base(configuration, signInManager, userManager, mailerService, jobRunService, logger)
        {
            _geocodingService = geocodingService;
            _historicPhotoService = historicPhotoService;
            _historicPhotoLocationService = historicPhotoLocationService;
        }

        protected override async Task RunInternal()
        {
            ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

            for (var id = 1; id < 12000; ++id)
            {
                await ProcessArchiveItem(claimsPrincipal, "MPL", id.ToString(), "https://content.mpl.org/digital", "https://content.mpl.org/digital/api/collections/HstoricPho/items/{0}/false", "https://content.mpl.org/digital/collection/HstoricPho/id/{0}/rec/1");
            }
        }

        private async Task ProcessArchiveItem(ClaimsPrincipal claimsPrincipal, string collection, string id, string imageUrlPrefix, string apiUrl, string uiUrl)
        {
            string historicPhotoId = collection + "_" + id;
            string url = string.Format(apiUrl, id);

            try
            {
                HistoricPhoto historicPhoto = await _historicPhotoService.GetOne(claimsPrincipal, historicPhotoId, null);
                if (historicPhoto != null)
                {
                    _logger.LogDebug("Skipping existing historic photo: {HistoricPhotoId}", historicPhotoId);
                    return;
                }

                string rawResponse = null;
                using (HttpResponseMessage response = await _httpClient.GetAsync(url))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("Non-success status code {StatusCode} loading {Url}", response.StatusCode.ToString(), url);
                        return;
                    }
                    rawResponse = await response.Content.ReadAsStringAsync();
                }

                var archiveItem = JsonConvert.DeserializeObject<ArchiveItem>(rawResponse);

                var title = archiveItem.Fields.Where(x => x.Key == "title").FirstOrDefault()?.Value;
                var description = archiveItem.Fields.Where(x => x.Key == "descri").FirstOrDefault()?.Value;
                var date = archiveItem.Fields.Where(x => x.Key == "date").FirstOrDefault()?.Value;
                var place = archiveItem.Fields.Where(x => x.Key == "place").FirstOrDefault()?.Value;
                var currentAddress = archiveItem.Fields.Where(x => x.Key == "curren").FirstOrDefault()?.Value;
                var oldAddress = archiveItem.Fields.Where(x => x.Key == "old").FirstOrDefault()?.Value;

                Guid? historicPhotoLocationId = null;
                if (!string.IsNullOrEmpty(currentAddress))
                {
                    GeocodeResults bestResult = null;

                    var currentAddresses = currentAddress.Split(';').Select(x => x.Trim()).ToList();
                    foreach (string address in currentAddresses)
                    {
                        var geocodeResult = await _geocodingService.Geocode(address);

                        if (geocodeResult.Accuracy == Models.GeometryAccuracy.High)
                        {
                            bestResult = geocodeResult;
                            break;
                        }

                        if (geocodeResult.Accuracy == Models.GeometryAccuracy.Medium && (bestResult == null || bestResult.Accuracy == Models.GeometryAccuracy.Low))
                            bestResult = geocodeResult;

                        if (geocodeResult.Accuracy == Models.GeometryAccuracy.Low && bestResult == null)
                            bestResult = geocodeResult;

                        _logger.LogDebug("Geocoded {CurrentAddress}", currentAddress);
                    }

                    if (bestResult != null)
                    {
                        var historicPhotoLocations = await _historicPhotoLocationService.GetAll(claimsPrincipal, 0, 10, null, null, null, null, null, null, null, false, false, queryable => queryable.Where(x => x.Geometry.EqualsTopologically(bestResult.Geometry)));

                        if (historicPhotoLocations.Count > 0)
                        {
                            historicPhotoLocationId = historicPhotoLocations[0].Id;
                        }
                        else
                        {
                            HistoricPhotoLocation historicPhotoLocation = new HistoricPhotoLocation()
                            {
                                Id = Guid.NewGuid(),
                                Geometry = bestResult.Geometry,
                                Accuracy = bestResult.Accuracy,
                                Source = bestResult.Source,
                                LastGeocodeAttempt = DateTime.Now
                            };

                            GeographicUtilities.SetBounds(historicPhotoLocation, bestResult.Geometry);

                            await _historicPhotoLocationService.Create(claimsPrincipal, historicPhotoLocation);
                            historicPhotoLocationId = historicPhotoLocation.Id;
                        }
                    }
                }

                int? year = null;

                if (date != null)
                {
                    date = date.Replace("c.", "");
                    date = date.Trim();

                    if (date.Length >= 4 && (date.StartsWith("18") || date.StartsWith("19") || date.StartsWith("20")))
                    {
                        int tempYear = 0;
                        if (int.TryParse(date.Substring(0, 4), out tempYear))
                            year = tempYear;
                    }
                }

                historicPhoto = new HistoricPhoto()
                {
                    Id = historicPhotoId,
                    HistoricPhotoLocationId = historicPhotoLocationId,
                    Collection = collection,
                    Title = title,
                    Description = description,
                    Date = date,
                    Year = year,
                    Place = place,
                    CurrentAddress = currentAddress,
                    OldAddress = oldAddress,
                    ImageUrl = imageUrlPrefix + archiveItem.ImageUri,
                    Url = string.Format(uiUrl, id)
                };

                await _historicPhotoService.Create(claimsPrincipal, historicPhoto);
                ++_successCount;

                _logger.LogInformation("Processed {Url}", url);
            }
            catch (Exception ex)
            {
                ++_failureCount;

                _logger.LogError(ex, "Error processing {Url}", url);
            }
        }

        private class ArchiveItem
        {
            public string CollectionAlias { get; set; }
            public string CollectionName { get; set; }
            public string ContentType { get; set; }
            public string DownloadUri { get; set; }
            public List<ItemField> Fields { get; set; }
            public string Filename { get; set; }
            public string Id { get; set; }
            public int? ImageHeight { get; set; }
            public string ImageUri { get; set; }
            public int? ImageWidth { get; set; }
            public string Text { get; set; }
            public string ThumbnailUri { get; set; }
            public string Title { get; set; }
            public string Url { get; set; }
        }

        private class ItemField
        {
            public string Key { get; set; }
            public string Title { get; set; }
            public string Value { get; set; }
        }

    }
}
