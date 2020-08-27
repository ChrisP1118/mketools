using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Services;
using MkeTools.Web.Services.Data.Interfaces;
using MkeTools.Web.Services.Functional;
using MkeTools.Web.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;

namespace MkeTools.Web.Jobs
{
    public abstract class ImportXmlJob<TDataModel> : LoggedJob
        where TDataModel : class, new()
    {
        private readonly IEntityWriteService<TDataModel, string> _writeService;

        public ImportXmlJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<ImportXmlJob<TDataModel>> logger, IEntityWriteService<TDataModel, string> writeService)
            : base(configuration, signInManager, userManager, mailerService, jobRunService, logger)
        {
            _writeService = writeService;
        }

        protected abstract void ProcessElement(TDataModel item, string elementName, string elementValue);

        protected virtual async Task BeforeSaveElement(TDataModel item)
        {
        }

        protected abstract string PackageName { get; }
        protected abstract string PackageFormat { get; }

        protected virtual bool UseBulkInsert => true;

        protected int ParseInt(string value, int defaultValue = 0)
        {
            return ParsingUtilities.ParseInt(value, defaultValue);
        }

        protected override async Task RunInternal()
        {
            string fileName = await PackageUtilities.DownloadPackageFile(_logger, PackageName, PackageFormat);

            _logger.LogDebug("Download complete: {Filename}", fileName);

            ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

            List<TDataModel> items = new List<TDataModel>();

            TDataModel item = null;
            string currentElement = null;
            int i = 0;

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
                                    Tuple<IEnumerable<TDataModel>, IEnumerable<TDataModel>> results1 = await _writeService.BulkUpsert(claimsPrincipal, items, UseBulkInsert);
                                    _successCount += results1.Item1.Count();
                                    _failureCount += results1.Item2.Count();

                                    _logger.LogDebug("Bulk inserted items at mod {Index}", i);
                                }
                                catch (Exception ex)
                                {
                                    _failureCount += items.Count;

                                    _logger.LogError(ex, "Error bulk inserting items at mod {Index}", i);
                                }
                                items.Clear();
                            }
                        }
                    }
                }

                try
                {
                    Tuple<IEnumerable<TDataModel>, IEnumerable<TDataModel>> results2 = await _writeService.BulkUpsert(claimsPrincipal, items, UseBulkInsert);
                    _successCount += results2.Item1.Count();
                    _failureCount += results2.Item2.Count();

                    _logger.LogDebug("Bulk inserted items at mod {Index}", i);
                }
                catch (Exception ex)
                {
                    _failureCount += items.Count;

                    _logger.LogError(ex, "Error bulk inserting items at mod {Index}", i);
                }
            }

            File.Delete(fileName);
        }
    }
}