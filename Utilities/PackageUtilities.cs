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
            try
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
                    if (response.Content.Headers.ContentLength.HasValue)
                    {
                        double contentLengthMB = (double)response.Content.Headers.ContentLength.Value / (double)1048576;
                        logger.LogInformation("Package size: " + contentLengthMB.ToString("0.00"));
                    }
                    
                    //await streamToReadFrom.CopyToAsync(streamToWriteTo);
                    await streamToReadFrom.CopyToWithProgressAsync(streamToWriteTo, 1048576, i =>
                    {
                        double mb = (double)i / (double)1048576;
                        logger.LogInformation("Downloaded " + mb.ToString("0.00") + " MB");
                    });
                }

                logger.LogTrace("File downloaded");

                return tempFileName;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error downloading package");
                throw;
            }
        }

        // https://stackoverflow.com/questions/36993171/how-to-get-a-copy-percent-when-doing-a-stream-copytoanotherstream-in-net
        public static async Task CopyToWithProgressAsync(this Stream source, Stream destination, int bufferSize = 16384, Action<long> progress = null)
        {
            var buffer = new byte[bufferSize];
            var total = 0L;
            int amtRead;
            do
            {
                amtRead = 0;
                while (amtRead < bufferSize)
                {
                    var numBytes = await source.ReadAsync(buffer,
                                                          amtRead,
                                                          bufferSize - amtRead);
                    if (numBytes == 0)
                    {
                        break;
                    }
                    amtRead += numBytes;
                }
                total += amtRead;
                await destination.WriteAsync(buffer, 0, amtRead);

                progress?.Invoke(total);
            } while (amtRead == bufferSize);
        }
    }
}
