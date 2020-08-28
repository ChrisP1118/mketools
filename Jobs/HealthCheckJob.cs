using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Data;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Services.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Jobs
{
    public class HealthCheckJob
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HealthCheckJob> _logger;
        private readonly IMailerService _mailerService;

        public HealthCheckJob(ApplicationDbContext dbContext, IConfiguration configuration, ILogger<HealthCheckJob> logger, IMailerService mailerService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _logger = logger;
            _mailerService = mailerService;
        }

        public async Task Run()
        {
            //if (!await _dbContext.PoliceDispatchCalls
            //    .Where(x => x.ReportedDateTime > DateTime.Now.AddHours(-6))
            //    .AnyAsync())
            //{
            //    await _mailerService.SendAdminAlert("Police Dispatch Calls Failing", "The most recent police dispatch call is at least 6 hours old.");
            //}

            //if (!await _dbContext.FireDispatchCalls
            //    .Where(x => x.ReportedDateTime > DateTime.Now.AddHours(-6))
            //    .AnyAsync())
            //{
            //    await _mailerService.SendAdminAlert("Fire Dispatch Calls Failing", "The most recent fire dispatch call is at least 6 hours old.");
            //}
        }
    }
}
