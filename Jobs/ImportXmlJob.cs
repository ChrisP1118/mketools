using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;

namespace MkeAlerts.Web.Jobs
{
    public abstract class ImportXmlJob<TDataModel> : ImportJob
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

        protected abstract string GetFileName();

        protected abstract void ProcessElement(TDataModel item, string elementName, string elementValue);

        public async Task Run()
        {
            _logger.LogInformation("Starting job");

            ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

            string file = GetFileName();

            List<TDataModel> items = new List<TDataModel>();

            TDataModel item = null;
            string currentElement = null;
            int i = 0;

            int success = 0;
            int failure = 0;

            using (XmlTextReader xmlReader = new XmlTextReader(file))
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
                            items.Add(item);

                            ++i;

                            if (i % 100 == 0)
                            {
                                Tuple<IEnumerable<TDataModel>, IEnumerable<TDataModel>> results1 = await _writeService.BulkCreate(claimsPrincipal, items, true);
                                success += results1.Item1.Count();
                                failure += results1.Item2.Count();
                                items.Clear();
                            }
                        }
                    }
                }

                Tuple<IEnumerable<TDataModel>, IEnumerable<TDataModel>> results2 = await _writeService.BulkCreate(claimsPrincipal, items, true);
                success += results2.Item1.Count();
                failure += results2.Item2.Count();
            }

            _logger.LogInformation("Import results: " + success.ToString() + " success, " + failure.ToString() + " failure");
            _logger.LogInformation("Finishing job");
        }
    }
}
