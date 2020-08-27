using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Data.Incidents;
using MkeTools.Web.Services;
using MkeTools.Web.Services.Data.Interfaces;
using MkeTools.Web.Services.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Jobs
{
    public class ImportCrimesArchiveJob : ImportCrimesJob
    {
        public ImportCrimesArchiveJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<ImportCrimesArchiveJob> logger, IEntityWriteService<Crime, string> writeService, IGeocodingService geocodingService) :
            base(configuration, signInManager, userManager, mailerService, jobRunService, logger, writeService, geocodingService)
        {
        }
        protected override string PackageName => "wibrarchive";

    }
}
