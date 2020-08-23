using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.AppHealth;
using MkeAlerts.Web.Services.Data.Interfaces;
using MkeAlerts.Web.Services.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Jobs
{
    public class ImportHistoricPhotosJob : LoggedJob
    {
        private static HttpClient _httpClient = new HttpClient();

        public ImportHistoricPhotosJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<LoggedJob> logger) :
            base(configuration, signInManager, userManager, mailerService, jobRunService, logger)
        {
        }

        protected override async Task RunInternal()
        {
            await CrawlListPage(@"https://content.mpl.org/digital/collection/HstoricPho/search/order/title/ad/asc");
        }

        private async Task CrawlListPage(string url)
        {
            _logger.LogInformation("Crawling list page: {Url}", url);

            string rawHtml = null;

            using (HttpResponseMessage response = await _httpClient.GetAsync(url))
            {
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Non-success status code {StatusCode} loading {Url}", response.StatusCode.ToString(), url);
                    return;
                }
                rawHtml = await response.Content.ReadAsStringAsync();
            }

            if (string.IsNullOrEmpty(rawHtml))
                return;

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(rawHtml);

            foreach (HtmlNode linkTag in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
            {
                string hrefValue = linkTag.GetAttributeValue("href", string.Empty);
                if (hrefValue.StartsWith("https://content.mpl.org/digital/collection/HstoricPho/id/", StringComparison.OrdinalIgnoreCase))
                {
                    await CrawlItemPage(hrefValue);
                }
            }
        }

        private async Task CrawlItemPage(string url)
        {
            _logger.LogInformation("Crawling item page: {Url}", url);

            string rawHtml = null;

            using (HttpResponseMessage response = await _httpClient.GetAsync(url))
            {
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Non-success status code {StatusCode} loading {Url}", response.StatusCode.ToString(), url);
                    return;
                }
                rawHtml = await response.Content.ReadAsStringAsync();
            }

            if (string.IsNullOrEmpty(rawHtml))
                return;

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(rawHtml);

            var itemImage = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='ItemImage-itemImage']//img");
            if (itemImage == null)
                return;

            string imageHref = itemImage.GetAttributeValue("href", null);

            var itemMetadataTable = htmlDoc.DocumentNode.SelectSingleNode("//table[@class='ItemView-itemMetadata']");
            if (itemMetadataTable == null)
                return;

            var itemTitle = GetMetadataValue(itemMetadataTable, "title");
            var itemDescription = GetMetadataValue(itemMetadataTable, "descri");
        }

        private string GetMetadataValue(HtmlNode metadataTable, string name)
        {
            var itemTitle = metadataTable.SelectSingleNode("/tr[@class='field-" + name + "']/td[@class='field-value']/span");
            return itemTitle.InnerText;

        }
    }
}
