using DotSpatial.Projections;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Services;
using MkeAlerts.Web.Services.Data;
using MkeAlerts.Web.Services.Data.Interfaces;
using MkeAlerts.Web.Services.Functional;
using MkeAlerts.Web.Utilities;
using NetTopologySuite;
using NetTopologySuite.Features;
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
using Parcel = MkeAlerts.Web.Models.Data.Places.Parcel;

namespace MkeAlerts.Web.Jobs
{
    public class ImportParcelsJob : ImportShapefileJob<Parcel, string>
    {
        public ImportParcelsJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<ImportParcelsJob> logger, IEntityWriteService<Parcel, string> writeService)
            : base(configuration, signInManager, userManager, mailerService, jobRunService, logger, writeService)
        {
            _shapefileName = configuration.GetValue<string>("ParcelShapefile");
        }

        protected override bool VerifyItem(IShapefileFeature source, Parcel target)
        {
            if (string.IsNullOrEmpty(target.TAXKEY))
                return false;

            target.HouseNumber = ParsingUtilities.ParseInt(target.HOUSENR, 0, true);
            target.HouseNumberHigh = ParsingUtilities.ParseInt(target.HOUSENRHI, 0, true);

            return true;
        }
    }

    public class ImportCommonParcelsJob : ImportShapefileJob<CommonParcel, int>
    {
        public ImportCommonParcelsJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<ImportCommonParcelsJob> logger, IEntityWriteService<CommonParcel, int> writeService)
            : base(configuration, signInManager, userManager, mailerService, jobRunService, logger, writeService)
        {
            _shapefileName = configuration.GetValue<string>("ParcelShapefile");
        }

        protected override bool VerifyItem(IShapefileFeature source, CommonParcel target)
        {
            if (target.MAP_ID == 0)
                return false;

            return true;
        }
    }
}
