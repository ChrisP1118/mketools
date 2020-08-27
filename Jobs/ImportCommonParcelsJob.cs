using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Services;
using MkeTools.Web.Services.Data.Interfaces;
using MkeTools.Web.Services.Functional;
using NetTopologySuite.IO.ShapeFile.Extended.Entities;

namespace MkeTools.Web.Jobs
{
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
