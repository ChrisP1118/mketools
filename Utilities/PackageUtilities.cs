using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Utilities
{
    public static class PackageUtilities
    {
        public static async Task<string> DownloadPackageFile(ILogger logger, string packageName, string format)
        {
            string packageDetailsUrl = "https://data.milwaukee.gov/api/3/action/package_show?id=" + packageName;
            string packageDetailsData = null;

            logger.LogTrace("Downloading package data from " + packageDetailsUrl);

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(packageDetailsUrl))
            {
                response.EnsureSuccessStatusCode();
                packageDetailsData = await response.Content.ReadAsStringAsync();
            }

            JObject dataObject = JObject.Parse(packageDetailsData);
            string packageFileUrl = (string)dataObject.SelectToken("result.resources[?(@.format == '" + format + "')].url");

            string tempFileName = Path.GetTempFileName();

            logger.LogInformation("Downloading package file " + packageFileUrl + " to " + tempFileName);

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(packageFileUrl, HttpCompletionOption.ResponseHeadersRead))
            using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
            using (Stream streamToWriteTo = File.Open(tempFileName, FileMode.Create))
            {
                await streamToReadFrom.CopyToAsync(streamToWriteTo);
            }

            logger.LogTrace("File downloaded");

            return tempFileName;
        }
    }
}
