using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Properties;
using MkeAlerts.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;

namespace MkeAlerts.Web.Jobs
{
    public class ImportPropertiesJob : ImportJob
    {
        private readonly ILogger<ImportPropertiesJob> _logger;
        private readonly IEntityWriteService<Property, string> _propertyWriteService;

        public ImportPropertiesJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<ImportPropertiesJob> logger, IEntityWriteService<Property, string> propertyWriteService)
            : base(configuration, signInManager, userManager)
        {
            _logger = logger;
            _propertyWriteService = propertyWriteService;
        }

        public async Task Run()
        {
            _logger.LogInformation("Starting job");

            ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

            string file = @"M:\My Documents\GitHub\mkealerts\DataSources\mprop\mprop.xml";

            List<Property> properties = new List<Property>();
            Property property = null;
            string currentElement = null;
            int i = 0;

            int success = 0;
            int failure = 0;

            using (XmlTextReader xmlReader = new XmlTextReader(file))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "element")
                        property = new Property();
                    else if (xmlReader.NodeType == XmlNodeType.Element)
                        currentElement = xmlReader.Name;

                    if (xmlReader.NodeType == XmlNodeType.Text)
                    {
                        switch (currentElement)
                        {
                            case "AIR_CONDITIONING": property.AIR_CONDITIONING = xmlReader.Value; break;
                            case "ATTIC": property.ATTIC = xmlReader.Value; break;
                            case "BASEMENT": property.BASEMENT = xmlReader.Value; break;
                            case "BATHS": property.BATHS = int.Parse(xmlReader.Value); break;
                            case "BEDROOMS": property.BEDROOMS = int.Parse(xmlReader.Value); break;
                            case "BLDG_AREA": property.BLDG_AREA = int.Parse(xmlReader.Value); break;
                            case "BLDG_TYPE": property.BLDG_TYPE = xmlReader.Value; break;
                            case "C_A_CLASS": property.C_A_CLASS = xmlReader.Value; break;
                            case "C_A_EXM_IMPRV": property.C_A_EXM_IMPRV = int.Parse(xmlReader.Value); break;
                            case "C_A_EXM_LAND": property.C_A_EXM_LAND = int.Parse(xmlReader.Value); break;
                            case "C_A_EXM_TOTAL": property.C_A_EXM_TOTAL = int.Parse(xmlReader.Value); break;
                            case "C_A_EXM_TYPE": property.C_A_EXM_TYPE = xmlReader.Value; break;
                            case "C_A_IMPRV": property.C_A_IMPRV = int.Parse(xmlReader.Value); break;
                            case "C_A_LAND": property.C_A_LAND = int.Parse(xmlReader.Value); break;
                            case "C_A_SYMBOL": property.C_A_SYMBOL = xmlReader.Value; break;
                            case "C_A_TOTAL": property.C_A_TOTAL = int.Parse(xmlReader.Value); break;
                            case "CHG_NR": property.CHG_NR = xmlReader.Value; break;
                            case "CHK_DIGIT": property.CHK_DIGIT = xmlReader.Value; break;
                            case "CONVEY_DATE": property.CONVEY_DATE = DateTime.Parse(xmlReader.Value); break;
                            case "CONVEY_FEE": property.CONVEY_FEE = float.Parse(xmlReader.Value); break;
                            case "CONVEY_TYPE": property.CONVEY_TYPE = xmlReader.Value; break;
                            case "SDIR": property.SDIR = xmlReader.Value; break;
                            case "DIV_DROP": property.DIV_DROP = int.Parse(xmlReader.Value); break;
                            case "DIV_ORG": property.DIV_ORG = int.Parse(xmlReader.Value); break;
                            case "DPW_SANITATION": property.DPW_SANITATION = xmlReader.Value; break;
                            case "EXM_ACREAGE": property.EXM_ACREAGE = float.Parse(xmlReader.Value); break;
                            case "EXM_PER_CT_IMPRV": property.EXM_PER_CT_IMPRV = float.Parse(xmlReader.Value); break;
                            case "EXM_PER_CT_LAND": property.EXM_PER_CT_LAND = float.Parse(xmlReader.Value); break;
                            case "FIREPLACE": property.FIREPLACE = xmlReader.Value; break;
                            case "GARAGE_TYPE": property.GARAGE_TYPE = xmlReader.Value; break;
                            case "GEO_ALDER": property.GEO_ALDER = int.Parse(xmlReader.Value); break;
                            case "GEO_ALDER_OLD": property.GEO_ALDER_OLD = int.Parse(xmlReader.Value); break;
                            case "GEO_BI_MAINT": property.GEO_BI_MAINT = int.Parse(xmlReader.Value); break;
                            case "GEO_BLOCK": property.GEO_BLOCK = xmlReader.Value; break;
                            case "GEO_FIRE": property.GEO_FIRE = int.Parse(xmlReader.Value); break;
                            case "GEO_POLICE": property.GEO_POLICE = int.Parse(xmlReader.Value); break;
                            case "GEO_TRACT": property.GEO_TRACT = int.Parse(xmlReader.Value); break;
                            case "GEO_ZIP_CODE": property.GEO_ZIP_CODE = int.Parse(xmlReader.Value); break;
                            case "HIST_CODE": property.HIST_CODE = xmlReader.Value; break;
                            case "HOUSE_NR_HI": property.HOUSE_NR_HI = int.Parse(xmlReader.Value); break;
                            case "HOUSE_NR_LO": property.HOUSE_NR_LO = int.Parse(xmlReader.Value); break;
                            case "HOUSE_NR_SFX": property.HOUSE_NR_SFX = xmlReader.Value; break;
                            case "LAND_USE": property.LAND_USE = int.Parse(xmlReader.Value); break;
                            case "LAND_USE_GP": property.LAND_USE_GP = int.Parse(xmlReader.Value); break;
                            case "LAST_NAME_CHG": property.LAST_NAME_CHG = DateTime.Parse(xmlReader.Value); break;
                            case "LAST_VALUE_CHG": property.LAST_VALUE_CHG = DateTime.Parse(xmlReader.Value); break;
                            case "LOT_AREA": property.LOT_AREA = int.Parse(xmlReader.Value); break;
                            case "NEIGHBORHOOD": property.NEIGHBORHOOD = xmlReader.Value; break;
                            case "NR_ROOMS": property.NR_ROOMS = xmlReader.Value; break;
                            case "NR_STORIES": property.NR_STORIES = float.Parse(xmlReader.Value); break;
                            case "NR_UNITS": property.NR_UNITS = int.Parse(xmlReader.Value); break;
                            case "NUMBER_OF_SPACES": property.NUMBER_OF_SPACES = int.Parse(xmlReader.Value); break;
                            case "OWNER_CITY_STATE": property.OWNER_CITY_STATE = xmlReader.Value; break;
                            case "OWNER_MAIL_ADDR": property.OWNER_MAIL_ADDR = xmlReader.Value; break;
                            case "OWNER_NAME_1": property.OWNER_NAME_1 = xmlReader.Value; break;
                            case "OWNER_NAME_2": property.OWNER_NAME_2 = xmlReader.Value; break;
                            case "OWNER_NAME_3": property.OWNER_NAME_3 = xmlReader.Value; break;
                            case "OWNER_ZIP": property.OWNER_ZIP = xmlReader.Value; break;
                            case "OWN_OCPD": property.OWN_OCPD = xmlReader.Value; break;
                            case "P_A_CLASS": property.P_A_CLASS = xmlReader.Value; break;
                            case "P_A_EXM_IMPRV": property.P_A_EXM_IMPRV = int.Parse(xmlReader.Value); break;
                            case "P_A_EXM_LAND": property.P_A_EXM_LAND = int.Parse(xmlReader.Value); break;
                            case "P_A_EXM_TOTAL": property.P_A_EXM_TOTAL = int.Parse(xmlReader.Value); break;
                            case "P_A_EXM_TYPE": property.P_A_EXM_TYPE = xmlReader.Value; break;
                            case "P_A_IMPRV": property.P_A_IMPRV = int.Parse(xmlReader.Value); break;
                            case "P_A_LAND": property.P_A_LAND = int.Parse(xmlReader.Value); break;
                            case "P_A_SYMBOL": property.P_A_SYMBOL = xmlReader.Value; break;
                            case "P_A_TOTAL": property.P_A_TOTAL = int.Parse(xmlReader.Value); break;
                            case "PLAT_PAGE": property.PLAT_PAGE = int.Parse(xmlReader.Value); break;
                            case "POWDER_ROOMS": property.POWDER_ROOMS = int.Parse(xmlReader.Value); break;
                            case "RAZE_STATUS": property.RAZE_STATUS = int.Parse(xmlReader.Value); break;
                            case "REASON_FOR_CHG": property.REASON_FOR_CHG = xmlReader.Value; break;
                            case "STREET": property.STREET = xmlReader.Value; break;
                            case "STTYPE": property.STTYPE = xmlReader.Value; break;
                            case "SUB_ACCT": property.SUB_ACCT = int.Parse(xmlReader.Value); break;
                            case "SWIM_POOL": property.SWIM_POOL = xmlReader.Value; break;
                            case "TAXKEY": property.TAXKEY = xmlReader.Value; break;
                            case "TAX_RATE_CD": property.TAX_RATE_CD = xmlReader.Value; break;
                            case "TOT_UNABATED": property.TOT_UNABATED = xmlReader.Value; break;
                            case "YEARS_DELQ": property.YEARS_DELQ = int.Parse(xmlReader.Value); break;
                            case "YR_ASSMT": property.YR_ASSMT = xmlReader.Value; break;
                            case "YR_BUILT": property.YR_BUILT = int.Parse(xmlReader.Value); break;
                            case "ZONING": property.ZONING = xmlReader.Value; break;
                            case "PARKING_SPACES": property.PARKING_SPACES = int.Parse(xmlReader.Value); break;
                            case "PARKING_TYPE": property.PARKING_TYPE = xmlReader.Value; break;
                            case "CORNER_LOT": property.CORNER_LOT = xmlReader.Value; break;
                            case "ANGLE": property.ANGLE = int.Parse(xmlReader.Value); break;
                            case "TAX_DELQ": property.TAX_DELQ = int.Parse(xmlReader.Value); break;
                            case "BI_VIOL": property.BI_VIOL = xmlReader.Value; break;
                        }
                    }

                    if (xmlReader.NodeType == XmlNodeType.EndElement)
                    {
                        if (xmlReader.Name == "element")
                        {
                            properties.Add(property);

                            ++i;

                            if (i % 100 == 0)
                            {
                                Tuple<IEnumerable<Property>, IEnumerable<Property>> results1 = await _propertyWriteService.BulkCreate(claimsPrincipal, properties, true);
                                success += results1.Item1.Count();
                                failure += results1.Item2.Count();
                                properties.Clear();
                            }
                        }
                    }
                }

                Tuple<IEnumerable<Property>, IEnumerable<Property>> results2 = await _propertyWriteService.BulkCreate(claimsPrincipal, properties, true);
                success += results2.Item1.Count();
                failure += results2.Item2.Count();
            }
        }
    }
}