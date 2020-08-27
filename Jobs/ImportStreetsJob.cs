using DotSpatial.Projections;
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
using System.Threading.Tasks;

namespace MkeTools.Web.Jobs
{
    public class ImportStreetsJob : ImportShapefileJob<Street, int>
    {
        public ImportStreetsJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<ImportStreetsJob> logger, IEntityWriteService<Street, int> writeService)
            : base(configuration, signInManager, userManager, mailerService, jobRunService, logger, writeService)
        {
            _shapefileName = configuration.GetValue<string>("StreetShapefile");
        }

        protected override bool VerifyItem(IShapefileFeature source, Street target)
        {
            if (target.CLINEID == 0)
                return false;

            target.LeftNumberLow = ParsingUtilities.ParseInt(target.LEFTFR, 0, true);
            target.LeftNumberHigh = ParsingUtilities.ParseInt(target.LEFTTO, 0, true);
            target.RightNumberLow = ParsingUtilities.ParseInt(target.RIGHTFR, 0, true);
            target.RightNumberHigh = ParsingUtilities.ParseInt(target.RIGHTTO, 0, true);

            return true;
        }
    }
}
