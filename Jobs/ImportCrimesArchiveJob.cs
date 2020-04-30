using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Incidents;
using MkeAlerts.Web.Services;
using MkeAlerts.Web.Services.Data.Interfaces;
using MkeAlerts.Web.Services.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Jobs
{
    public class ImportCrimesArchiveJob : ImportCrimesJob
    {
        public ImportCrimesArchiveJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<ImportCrimesArchiveJob> logger, IEntityWriteService<Crime, string> writeService) :
            base(configuration, signInManager, userManager, mailerService, jobRunService, logger, writeService)
        {
        }
        protected override string PackageName => "wibrarchive";

    }
}
