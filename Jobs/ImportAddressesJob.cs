using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
    public class ImportAddressesJob : ImportJob
    {
        private readonly IEntityWriteService<Address, string> _addressWriteService;

        public ImportAddressesJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEntityWriteService<Address, string> addressWriteService)
            : base(configuration, signInManager, userManager)
        {
            _addressWriteService = addressWriteService;
        }

        public async Task<string> Run()
        {
            ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

            string file = @"M:\My Documents\GitHub\mkealerts\DataSources\mai\mai.xml";

            List<Address> addresses = new List<Address>();

            Address address = null;
            string currentElement = null;
            int i = 0;

            using (XmlTextReader xmlReader = new XmlTextReader(file))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "element")
                        address = new Address();
                    else if (xmlReader.NodeType == XmlNodeType.Element)
                        currentElement = xmlReader.Name;

                    if (xmlReader.NodeType == XmlNodeType.Text)
                    {
                        switch (currentElement)
                        {
                            case "TAXKEY":
                                address.TAXKEY = xmlReader.Value;
                                break;
                            case "HSE_NBR":
                                address.HSE_NBR = int.Parse(xmlReader.Value);
                                break;
                            case "SFX":
                                address.SFX = xmlReader.Value;
                                break;
                            case "DIR":
                                address.DIR = xmlReader.Value;
                                break;
                            case "STREET":
                                address.STREET = xmlReader.Value;
                                break;
                            case "STTYPE":
                                address.STTYPE = xmlReader.Value;
                                break;
                            case "UNIT_NBR":
                                address.UNIT_NBR = xmlReader.Value;
                                break;
                            case "ZIP_CODE":
                                address.ZIP_CODE = xmlReader.Value;
                                break;
                            case "LAND_USE":
                                address.LAND_USE = int.Parse(xmlReader.Value);
                                break;
                            case "RCD_NBR":
                                address.RCD_NBR = int.Parse(xmlReader.Value);
                                break;
                            case "UPD_DATE":
                                address.UPD_DATE = int.Parse(xmlReader.Value);
                                break;
                            case "WARD":
                                address.WARD = int.Parse(xmlReader.Value);
                                break;
                            case "MAIL_ERROR_COUNT":
                                address.MAIL_ERROR_COUNT = int.Parse(xmlReader.Value);
                                break;
                            case "MAIL_STATUS":
                                address.MAIL_STATUS = xmlReader.Value;
                                break;
                            case "RES_COM_FLAG":
                                address.RES_COM_FLAG = xmlReader.Value;
                                break;
                        }
                    }

                    if (xmlReader.NodeType == XmlNodeType.EndElement)
                    {
                        if (xmlReader.Name == "element")
                        {
                            address.BuildFormattedAddress();
                            addresses.Add(address);

                            ++i;

                            if (i % 100 == 0)
                            {
                                await _addressWriteService.BulkCreate(claimsPrincipal, addresses, true);
                                addresses.Clear();
                            }
                        }
                    }
                }

                await _addressWriteService.BulkCreate(claimsPrincipal, addresses, true);
            }

            return null;
        }
    }
}