using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Functional
{
    public class PickupDatesService : IPickupDatesService
    {
        protected readonly IConfiguration _configuration;
        protected readonly ILogger<PickupDatesService> _logger;

        public PickupDatesService(IConfiguration configuration, ILogger<PickupDatesService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<PickupDatesResults> GetPickupDates(string laddr, string sdir, string sname, string stype)
        {
            PickupDatesResults pickupDatesResults = new PickupDatesResults();
            string pageUrl = _configuration["PickupDatesUrl"];
            string result = null;

            FormUrlEncodedContent postContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("laddr", laddr),
                new KeyValuePair<string, string>("sdir", sdir),
                new KeyValuePair<string, string>("sname", sname),
                new KeyValuePair<string, string>("stype", stype),
                new KeyValuePair<string, string>("embed", "Y"),
                new KeyValuePair<string, string>("Submit", "Submit")
            });

            try
            {
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.PostAsync(pageUrl, postContent))
                using (HttpContent content = response.Content)
                {
                    result = await content.ReadAsStringAsync();
                }

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(result);

                string garbageDate = htmlDocument.DocumentNode
                    .Descendants()
                    .Where(n => n.Name == "strong")
                    .Where(n => n.PreviousSibling != null && n.PreviousSibling.InnerText.Contains("The next garbage collection pickup for this location is:"))
                    .Select(n => n.InnerText)
                    .FirstOrDefault();

                string recyclingDate = htmlDocument.DocumentNode
                    .Descendants()
                    .Where(n => n.Name == "strong")
                    .Where(n => n.PreviousSibling != null && n.PreviousSibling.InnerText.Contains("The next recycling collection pickup for this location is"))
                    .Select(n => n.InnerText)
                    .FirstOrDefault();

                DateTime parsedGarbageDate;
                DateTime parsedRecyclingDate;
                if (garbageDate != null && DateTime.TryParse(garbageDate, out parsedGarbageDate))
                    pickupDatesResults.NextGarbagePickupDate = parsedGarbageDate;
                if (recyclingDate != null && DateTime.TryParse(recyclingDate, out parsedRecyclingDate))
                    pickupDatesResults.NextRecyclingPickupDate = parsedRecyclingDate;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting pickup dates for {laddr} {sdir} {sname} {stype}", ex);
            }

            return pickupDatesResults;
        }
    }
}
