using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Services;
using MkeTools.Web.Services.Data.Interfaces;
using MkeTools.Web.Services.Functional;
using MkeTools.Web.Utilities;
using NetTopologySuite.IO.ShapeFile.Extended.Entities;
using Parcel = MkeTools.Web.Models.Data.Places.Parcel;

namespace MkeTools.Web.Jobs
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
}
