using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Services;
using MkeAlerts.Web.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;

namespace MkeAlerts.Web.Jobs
{
    public abstract class ImportXmlJob<TDataModel> : Job
        where TDataModel : class, new()
    {
        private readonly ILogger<ImportXmlJob<TDataModel>> _logger;
        private readonly IEntityWriteService<TDataModel, string> _writeService;

        public ImportXmlJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<ImportXmlJob<TDataModel>> logger, IEntityWriteService<TDataModel, string> writeService)
            : base(configuration, signInManager, userManager)
        {
            _logger = logger;
            _writeService = writeService;
        }

        protected abstract void ProcessElement(TDataModel item, string elementName, string elementValue);

        protected virtual async Task BeforeSaveElement(TDataModel item)
        {
        }

        protected abstract string PackageName { get; }
        protected abstract string PackageFormat { get; }

        protected virtual bool UseBulkInsert => true;

        public async Task Run()
        {
            _logger.LogInformation("Starting job");

            string fileName = await PackageUtilities.DownloadPackageFile(_logger, PackageName, PackageFormat);

            _logger.LogDebug("Download complete: " + fileName);

            ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

            List<TDataModel> items = new List<TDataModel>();

            TDataModel item = null;
            string currentElement = null;
            int i = 0;

            int success = 0;
            int failure = 0;

            using (XmlTextReader xmlReader = new XmlTextReader(fileName))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "element")
                        item = new TDataModel();
                    else if (xmlReader.NodeType == XmlNodeType.Element)
                        currentElement = xmlReader.Name;

                    if (xmlReader.NodeType == XmlNodeType.Text)
                        ProcessElement(item, currentElement, xmlReader.Value);

                    if (xmlReader.NodeType == XmlNodeType.EndElement)
                    {
                        if (xmlReader.Name == "element")
                        {
                            await BeforeSaveElement(item);
                            items.Add(item);

                            ++i;

                            if (i % 100 == 0)
                            {
                                try
                                {
                                    Tuple<IEnumerable<TDataModel>, IEnumerable<TDataModel>> results1 = await _writeService.BulkCreate(claimsPrincipal, items, UseBulkInsert);
                                    success += results1.Item1.Count();
                                    failure += results1.Item2.Count();

                                    _logger.LogDebug("Bulk inserted items at mod " + i.ToString());
                                }
                                catch (Exception ex)
                                {
                                    failure += items.Count;

                                    _logger.LogError(ex, "Error bulk inserting items at mod " + i.ToString());
                                }
                                items.Clear();
                            }
                        }
                    }
                }

                try
                {
                    Tuple<IEnumerable<TDataModel>, IEnumerable<TDataModel>> results2 = await _writeService.BulkCreate(claimsPrincipal, items, UseBulkInsert);
                    success += results2.Item1.Count();
                    failure += results2.Item2.Count();

                    _logger.LogDebug("Bulk inserted items at mod " + i.ToString());
                }
                catch (Exception ex)
                {
                    failure += items.Count;

                    _logger.LogError(ex, "Error bulk inserting items at mod " + i.ToString());
                }
            }

            _logger.LogInformation("Import results: " + success.ToString() + " success, " + failure.ToString() + " failure");

            File.Delete(fileName);

            _logger.LogInformation("Finishing job");
        }
    }
}
