using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Services;
using MkeAlerts.Web.Services.Data.Interfaces;
using MkeAlerts.Web.Services.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;

namespace MkeAlerts.Web.Jobs
{
    public class ImportPropertiesJob : ImportXmlJob<Property>
    {
        public ImportPropertiesJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<ImportPropertiesJob> logger, IEntityWriteService<Property, string> writeService) :
            base(configuration, signInManager, userManager, mailerService, jobRunService, logger, writeService)
        {
        }

        protected override string PackageName => "mprop";
        protected override string PackageFormat => "XML";

        protected override void ProcessElement(Property item, string elementName, string elementValue)
        {
            switch (elementName)
            {
                case "AIR_CONDITIONING": item.AIR_CONDITIONING = elementValue; break;
                case "ATTIC": item.ATTIC = elementValue; break;
                case "BASEMENT": item.BASEMENT = elementValue; break;
                case "BATHS": item.BATHS = ParseInt(elementValue); break;
                case "BEDROOMS": item.BEDROOMS = ParseInt(elementValue); break;
                case "BLDG_AREA": item.BLDG_AREA = ParseInt(elementValue); break;
                case "BLDG_TYPE": item.BLDG_TYPE = elementValue; break;
                case "C_A_CLASS": item.C_A_CLASS = elementValue; break;
                case "C_A_EXM_IMPRV": item.C_A_EXM_IMPRV = ParseInt(elementValue); break;
                case "C_A_EXM_LAND": item.C_A_EXM_LAND = ParseInt(elementValue); break;
                case "C_A_EXM_TOTAL": item.C_A_EXM_TOTAL = ParseInt(elementValue); break;
                case "C_A_EXM_TYPE": item.C_A_EXM_TYPE = elementValue; break;
                case "C_A_IMPRV": item.C_A_IMPRV = ParseInt(elementValue); break;
                case "C_A_LAND": item.C_A_LAND = ParseInt(elementValue); break;
                case "C_A_SYMBOL": item.C_A_SYMBOL = elementValue; break;
                case "C_A_TOTAL": item.C_A_TOTAL = ParseInt(elementValue); break;
                case "CHG_NR": item.CHG_NR = elementValue; break;
                case "CHK_DIGIT": item.CHK_DIGIT = elementValue; break;
                case "CONVEY_DATE": item.CONVEY_DATE = DateTime.Parse(elementValue); break;
                case "CONVEY_FEE": item.CONVEY_FEE = float.Parse(elementValue); break;
                case "CONVEY_TYPE": item.CONVEY_TYPE = elementValue; break;
                case "SDIR": item.SDIR = elementValue; break;
                case "DIV_DROP": item.DIV_DROP = ParseInt(elementValue); break;
                case "DIV_ORG": item.DIV_ORG = ParseInt(elementValue); break;
                case "DPW_SANITATION": item.DPW_SANITATION = elementValue; break;
                case "EXM_ACREAGE": item.EXM_ACREAGE = float.Parse(elementValue); break;
                case "EXM_PER_CT_IMPRV": item.EXM_PER_CT_IMPRV = float.Parse(elementValue); break;
                case "EXM_PER_CT_LAND": item.EXM_PER_CT_LAND = float.Parse(elementValue); break;
                case "FIREPLACE": item.FIREPLACE = elementValue; break;
                case "GARAGE_TYPE": item.GARAGE_TYPE = elementValue; break;
                case "GEO_ALDER": item.GEO_ALDER = ParseInt(elementValue); break;
                case "GEO_ALDER_OLD": item.GEO_ALDER_OLD = ParseInt(elementValue); break;
                case "GEO_BI_MAINT": item.GEO_BI_MAINT = ParseInt(elementValue); break;
                case "GEO_BLOCK": item.GEO_BLOCK = elementValue; break;
                case "GEO_FIRE": item.GEO_FIRE = ParseInt(elementValue); break;
                case "GEO_POLICE": item.GEO_POLICE = ParseInt(elementValue); break;
                case "GEO_TRACT": item.GEO_TRACT = ParseInt(elementValue); break;
                case "GEO_ZIP_CODE": item.GEO_ZIP_CODE = ParseInt(elementValue); break;
                case "HIST_CODE": item.HIST_CODE = elementValue; break;
                case "HOUSE_NR_HI": item.HOUSE_NR_HI = ParseInt(elementValue); break;
                case "HOUSE_NR_LO": item.HOUSE_NR_LO = ParseInt(elementValue); break;
                case "HOUSE_NR_SFX": item.HOUSE_NR_SFX = elementValue; break;
                case "LAND_USE": item.LAND_USE = ParseInt(elementValue); break;
                case "LAND_USE_GP": item.LAND_USE_GP = ParseInt(elementValue); break;
                case "LAST_NAME_CHG": item.LAST_NAME_CHG = DateTime.Parse(elementValue); break;
                case "LAST_VALUE_CHG": item.LAST_VALUE_CHG = DateTime.Parse(elementValue); break;
                case "LOT_AREA": item.LOT_AREA = ParseInt(elementValue); break;
                case "NEIGHBORHOOD": item.NEIGHBORHOOD = elementValue; break;
                case "NR_ROOMS": item.NR_ROOMS = elementValue; break;
                case "NR_STORIES": item.NR_STORIES = float.Parse(elementValue); break;
                case "NR_UNITS": item.NR_UNITS = ParseInt(elementValue); break;
                case "NUMBER_OF_SPACES": item.NUMBER_OF_SPACES = ParseInt(elementValue); break;
                case "OWNER_CITY_STATE": item.OWNER_CITY_STATE = elementValue; break;
                case "OWNER_MAIL_ADDR": item.OWNER_MAIL_ADDR = elementValue; break;
                case "OWNER_NAME_1": item.OWNER_NAME_1 = elementValue; break;
                case "OWNER_NAME_2": item.OWNER_NAME_2 = elementValue; break;
                case "OWNER_NAME_3": item.OWNER_NAME_3 = elementValue; break;
                case "OWNER_ZIP": item.OWNER_ZIP = elementValue; break;
                case "OWN_OCPD": item.OWN_OCPD = elementValue; break;
                case "P_A_CLASS": item.P_A_CLASS = elementValue; break;
                case "P_A_EXM_IMPRV": item.P_A_EXM_IMPRV = ParseInt(elementValue); break;
                case "P_A_EXM_LAND": item.P_A_EXM_LAND = ParseInt(elementValue); break;
                case "P_A_EXM_TOTAL": item.P_A_EXM_TOTAL = ParseInt(elementValue); break;
                case "P_A_EXM_TYPE": item.P_A_EXM_TYPE = elementValue; break;
                case "P_A_IMPRV": item.P_A_IMPRV = ParseInt(elementValue); break;
                case "P_A_LAND": item.P_A_LAND = ParseInt(elementValue); break;
                case "P_A_SYMBOL": item.P_A_SYMBOL = elementValue; break;
                case "P_A_TOTAL": item.P_A_TOTAL = ParseInt(elementValue); break;
                case "PLAT_PAGE": item.PLAT_PAGE = ParseInt(elementValue); break;
                case "POWDER_ROOMS": item.POWDER_ROOMS = ParseInt(elementValue); break;
                case "RAZE_STATUS": item.RAZE_STATUS = ParseInt(elementValue); break;
                case "REASON_FOR_CHG": item.REASON_FOR_CHG = elementValue; break;
                case "STREET": item.STREET = elementValue; break;
                case "STTYPE": item.STTYPE = elementValue; break;
                case "SUB_ACCT": item.SUB_ACCT = ParseInt(elementValue); break;
                case "SWIM_POOL": item.SWIM_POOL = elementValue; break;
                case "TAXKEY": item.TAXKEY = elementValue; break;
                case "TAX_RATE_CD": item.TAX_RATE_CD = elementValue; break;
                case "TOT_UNABATED": item.TOT_UNABATED = elementValue; break;
                case "YEARS_DELQ": item.YEARS_DELQ = ParseInt(elementValue); break;
                case "YR_ASSMT": item.YR_ASSMT = elementValue; break;
                case "YR_BUILT": item.YR_BUILT = ParseInt(elementValue); break;
                case "ZONING": item.ZONING = elementValue; break;
                case "PARKING_SPACES": item.PARKING_SPACES = ParseInt(elementValue); break;
                case "PARKING_TYPE": item.PARKING_TYPE = elementValue; break;
                case "CORNER_LOT": item.CORNER_LOT = elementValue; break;
                case "ANGLE": item.ANGLE = ParseInt(elementValue); break;
                case "TAX_DELQ": item.TAX_DELQ = ParseInt(elementValue); break;
                case "BI_VIOL": item.BI_VIOL = elementValue; break;
            }
        }
    }
}