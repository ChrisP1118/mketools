﻿using DotSpatial.Projections;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Services;
using MkeTools.Web.Services.Data.Interfaces;
using MkeTools.Web.Services.Functional;
using MkeTools.Web.Utilities;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.IO.ShapeFile.Extended;
using NetTopologySuite.IO.ShapeFile.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MkeTools.Web.Jobs
{
    public class ImportAddressesJob : ImportShapefileJob<Address, int>
    {
        public ImportAddressesJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<ImportAddressesJob> logger, IEntityWriteService<Address, int> writeService)
            : base(configuration, signInManager, userManager, mailerService, jobRunService, logger, writeService)
        {
            _shapefileName = configuration.GetValue<string>("AddressShapefile");
        }

        protected override bool VerifyItem(IShapefileFeature source, Address target)
        {
            if (target.ADDRESS_ID == 0)
                return false;

            int x = 0;
            int.TryParse(source.Attributes["HOUSENO"].ToString(), out x);
            target.HouseNumber = x;

            return true;
        }
    }
}
