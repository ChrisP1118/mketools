using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Models.Internal;
using MkeTools.Web.Services;
using MkeTools.Web.Services.Data;
using MkeTools.Web.Services.Data.Interfaces;
using MkeTools.Web.Services.Functional;
using MkeTools.Web.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;

namespace MkeTools.Web.Jobs
{
    public class ImportPropertiesJob : LoggedJob
    {
        private readonly IPropertyService _propertyService;
        private readonly IParcelService _parcelService;

        public ImportPropertiesJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<ImportPropertiesJob> logger, IPropertyService propertyService, IParcelService parcelService) :
            base(configuration, signInManager, userManager, mailerService, jobRunService, logger)
        {
            _propertyService = propertyService;
            _parcelService = parcelService;
        }

        protected string PackageName => "mprop";
        protected string PackageFormat => "XML";

        protected virtual bool UseBulkInsert => true;

        protected int ParseInt(string value, int defaultValue = 0)
        {
            return ParsingUtilities.ParseInt(value, defaultValue);
        }

        protected string EnforceLength(string value, int maxLength)
        {
            if (value.Length > maxLength)
                value = value.Substring(0, maxLength);

            return value;
        }

        protected override async Task RunInternal()
        {
            DateTime sourceDate = DateTime.Now;
            ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

            string fileName = await PackageUtilities.DownloadPackageFile(_logger, PackageName, PackageFormat);

            _logger.LogDebug("Download complete: {Filename}", fileName);

            _logger.LogInformation("Loading taxkeys");
            HashSet<string> taxkeys = await _parcelService.GetAllTaxkeys(claimsPrincipal);
            _logger.LogInformation("Loaded {Count} taxkeys", taxkeys.Count);

            _logger.LogInformation("Loading current property records");
            Dictionary<string, CurrentPropertyRecord> currentPropertyRecords = await _propertyService.GetCurrentRecords(claimsPrincipal);
            _logger.LogInformation("Loaded {Count} current property records", currentPropertyRecords.Count);

            List<Property> items = new List<Property>();
            Property item = null;
            string currentElement = null;
            int i = 0;

            using (XmlTextReader xmlReader = new XmlTextReader(fileName))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "element")
                        item = new Property()
                        {
                            Id = Guid.NewGuid(),
                            Source = "CurrentMprop",
                            SourceDate = sourceDate
                        };
                    else if (xmlReader.NodeType == XmlNodeType.Element)
                        currentElement = xmlReader.Name;

                    if (xmlReader.NodeType == XmlNodeType.Text)
                        ProcessElement(item, currentElement, xmlReader.Value);

                    if (xmlReader.NodeType == XmlNodeType.EndElement)
                    {
                        if (xmlReader.Name == "element")
                        {
                            ++i;

                            if (taxkeys.Contains(item.TAXKEY))
                            {
                                if (!currentPropertyRecords.ContainsKey(item.TAXKEY))
                                    items.Add(item);

                                else
                                {
                                    CurrentPropertyRecord currentPropertyRecord = currentPropertyRecords[item.TAXKEY];
                                    if (currentPropertyRecord.LAST_NAME_CHG != item.LAST_NAME_CHG || currentPropertyRecord.LAST_VALUE_CHG != item.LAST_VALUE_CHG)
                                        items.Add(item);
                                }
                            }

                            if (i % 100 == 0)
                            {
                                try
                                {
                                    Tuple<IEnumerable<Property>, IEnumerable<Property>> results1 = await _propertyService.BulkUpsert(claimsPrincipal, items, UseBulkInsert);
                                    _successCount += results1.Item1.Count();
                                    _failureCount += results1.Item2.Count();

                                    _logger.LogDebug("Bulk inserted {Count} items at mod {Index}", _successCount, i);
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
            }

            File.Delete(fileName);
        }

        protected void ProcessElement(Property item, string elementName, string elementValue)
        {
            switch (elementName)
            {
                case "AIR_CONDITIONING": item.AIR_CONDITIONING = EnforceLength(elementValue, 3); break;
                case "ATTIC": item.ATTIC = EnforceLength(elementValue, 1); break;
                case "BASEMENT": item.BASEMENT = EnforceLength(elementValue, 1); break;
                case "BATHS": item.BATHS = ParseInt(elementValue); break;
                case "BEDROOMS": item.BEDROOMS = ParseInt(elementValue); break;
                case "BLDG_AREA": item.BLDG_AREA = ParseInt(elementValue); break;
                case "BLDG_TYPE": item.BLDG_TYPE = EnforceLength(elementValue, 9); break;
                case "C_A_CLASS": item.C_A_CLASS = EnforceLength(elementValue, 4); break;
                case "C_A_EXM_IMPRV": item.C_A_EXM_IMPRV = ParseInt(elementValue); break;
                case "C_A_EXM_LAND": item.C_A_EXM_LAND = ParseInt(elementValue); break;
                case "C_A_EXM_TOTAL": item.C_A_EXM_TOTAL = ParseInt(elementValue); break;
                case "C_A_EXM_TYPE": item.C_A_EXM_TYPE = EnforceLength(elementValue, 3); break;
                case "C_A_IMPRV": item.C_A_IMPRV = ParseInt(elementValue); break;
                case "C_A_LAND": item.C_A_LAND = ParseInt(elementValue); break;
                case "C_A_SYMBOL": item.C_A_SYMBOL = EnforceLength(elementValue, 1); break;
                case "C_A_TOTAL": item.C_A_TOTAL = ParseInt(elementValue); break;
                case "CHK_DIGIT": item.CHK_DIGIT = EnforceLength(elementValue, 1); break;
                case "CONVEY_DATE": item.CONVEY_DATE = DateTime.Parse(elementValue); break;
                case "CONVEY_FEE": item.CONVEY_FEE = float.Parse(elementValue); break;
                case "CONVEY_TYPE": item.CONVEY_TYPE = EnforceLength(elementValue, 2); break;
                case "SDIR": item.SDIR = EnforceLength(elementValue, 1); break;
                case "DIV_ORG": item.DIV_ORG = ParseInt(elementValue); break;
                case "FIREPLACE": item.FIREPLACE = EnforceLength(elementValue, 1); break;
                case "GARAGE_TYPE": item.GARAGE_TYPE = EnforceLength(elementValue, 2); break;
                case "GEO_ALDER": item.GEO_ALDER = ParseInt(elementValue); break;
                case "GEO_ALDER_OLD": item.GEO_ALDER_OLD = ParseInt(elementValue); break;
                case "GEO_BLOCK": item.GEO_BLOCK = EnforceLength(elementValue, 4); break;
                case "GEO_POLICE": item.GEO_POLICE = ParseInt(elementValue); break;
                case "GEO_TRACT": item.GEO_TRACT = ParseInt(elementValue); break;
                case "GEO_ZIP_CODE": item.GEO_ZIP_CODE = ParseInt(elementValue); break;
                case "HIST_CODE": item.HIST_CODE = EnforceLength(elementValue, 1); break;
                case "HOUSE_NR_HI": item.HOUSE_NR_HI = ParseInt(elementValue); break;
                case "HOUSE_NR_LO": item.HOUSE_NR_LO = ParseInt(elementValue); break;
                case "HOUSE_NR_SFX": item.HOUSE_NR_SFX = EnforceLength(elementValue, 3); break;
                case "LAND_USE": item.LAND_USE = EnforceLength(elementValue, 4); break;
                case "LAND_USE_GP": item.LAND_USE_GP = EnforceLength(elementValue, 2); break;
                case "LAST_NAME_CHG": item.LAST_NAME_CHG = DateTime.Parse(elementValue); break;
                case "LAST_VALUE_CHG": item.LAST_VALUE_CHG = DateTime.Parse(elementValue); break;
                case "LOT_AREA": item.LOT_AREA = ParseInt(elementValue); break;
                case "NEIGHBORHOOD": item.NEIGHBORHOOD = EnforceLength(elementValue, 8); break;
                case "NR_STORIES": item.NR_STORIES = float.Parse(elementValue); break;
                case "NR_UNITS": item.NR_UNITS = ParseInt(elementValue); break;
                case "OWNER_CITY_STATE": item.OWNER_CITY_STATE = EnforceLength(elementValue, 28); break;
                case "OWNER_MAIL_ADDR": item.OWNER_MAIL_ADDR = EnforceLength(elementValue, 28); break;
                case "OWNER_NAME_1": item.OWNER_NAME_1 = EnforceLength(elementValue, 28); break;
                case "OWNER_NAME_2": item.OWNER_NAME_2 = EnforceLength(elementValue, 28); break;
                case "OWNER_NAME_3": item.OWNER_NAME_3 = EnforceLength(elementValue, 28); break;
                case "OWNER_ZIP": item.OWNER_ZIP = EnforceLength(elementValue, 9); break;
                case "OWN_OCPD": item.OWN_OCPD = EnforceLength(elementValue, 1); break;
                case "P_A_CLASS": item.P_A_CLASS = EnforceLength(elementValue, 1); break;
                case "P_A_EXM_IMPRV": item.P_A_EXM_IMPRV = ParseInt(elementValue); break;
                case "P_A_EXM_LAND": item.P_A_EXM_LAND = ParseInt(elementValue); break;
                case "P_A_EXM_TOTAL": item.P_A_EXM_TOTAL = ParseInt(elementValue); break;
                case "P_A_IMPRV": item.P_A_IMPRV = ParseInt(elementValue); break;
                case "P_A_LAND": item.P_A_LAND = ParseInt(elementValue); break;
                case "P_A_SYMBOL": item.P_A_SYMBOL = EnforceLength(elementValue, 4); break;
                case "P_A_TOTAL": item.P_A_TOTAL = ParseInt(elementValue); break;
                case "PLAT_PAGE": item.PLAT_PAGE = EnforceLength(elementValue, 5); break;
                case "POWDER_ROOMS": item.POWDER_ROOMS = ParseInt(elementValue); break;
                case "REASON_FOR_CHG": item.REASON_FOR_CHG = EnforceLength(elementValue, 3); break;
                case "STREET": item.STREET = EnforceLength(elementValue, 18); break;
                case "STTYPE": item.STTYPE = EnforceLength(elementValue, 2); break;
                case "TAXKEY": item.TAXKEY = EnforceLength(elementValue, 10); break;
                case "TAX_RATE_CD": item.TAX_RATE_CD = EnforceLength(elementValue, 2); break;
                case "YR_ASSMT": item.YR_ASSMT = EnforceLength(elementValue, 4); break;
                case "YR_BUILT": item.YR_BUILT = ParseInt(elementValue); break;
                case "ZONING": item.ZONING = EnforceLength(elementValue, 7); break;
            }
        }
    }
}