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
    public class ImportAddressesJob : ImportXmlJob<Address>
    {
        public ImportAddressesJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<ImportAddressesJob> logger, IEntityWriteService<Address, string> writeService) :
            base(configuration, signInManager, userManager, mailerService, jobRunService, logger, writeService)
        {
        }

        protected override string PackageName => "mai";
        protected override string PackageFormat => "XML";

        protected override void ProcessElement(Address item, string elementName, string elementValue)
        {
            switch (elementName)
            {
                case "TAXKEY":
                    item.TAXKEY = elementValue;
                    break;
                case "HSE_NBR":
                    item.HSE_NBR = int.Parse(elementValue);
                    break;
                case "SFX":
                    item.SFX = elementValue;
                    break;
                case "DIR":
                    item.DIR = elementValue;
                    break;
                case "STREET":
                    item.STREET = elementValue;
                    break;
                case "STTYPE":
                    item.STTYPE = elementValue;
                    break;
                case "UNIT_NBR":
                    item.UNIT_NBR = elementValue;
                    break;
                case "ZIP_CODE":
                    item.ZIP_CODE = elementValue;
                    break;
                case "LAND_USE":
                    item.LAND_USE = int.Parse(elementValue);
                    break;
                case "RCD_NBR":
                    item.RCD_NBR = elementValue;
                    break;
                case "UPD_DATE":
                    item.UPD_DATE = int.Parse(elementValue);
                    break;
                case "WARD":
                    item.WARD = int.Parse(elementValue);
                    break;
                case "MAIL_ERROR_COUNT":
                    item.MAIL_ERROR_COUNT = int.Parse(elementValue);
                    break;
                case "MAIL_STATUS":
                    item.MAIL_STATUS = elementValue;
                    break;
                case "RES_COM_FLAG":
                    item.RES_COM_FLAG = elementValue;
                    break;
            }
        }
    }
}