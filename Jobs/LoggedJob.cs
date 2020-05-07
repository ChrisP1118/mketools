using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.AppHealth;
using MkeAlerts.Web.Services.Data.Interfaces;
using MkeAlerts.Web.Services.Functional;
using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Jobs
{
    public abstract class LoggedJob : Job
    {
        protected readonly IJobRunService _jobRunService;
        protected readonly ILogger<LoggedJob> _logger;

        protected int _successCount = 0;
        protected int _failureCount = 0;

        public LoggedJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<LoggedJob> logger) :
            base(configuration, signInManager, userManager, mailerService)
        {
            _jobRunService = jobRunService;
            _logger = logger;
        }

        public async Task Run()
        {
            JobRun jobRun = new JobRun()
            {
                Id = Guid.NewGuid(),
                JobName = this.GetType().Name,
                StartTime = DateTimeOffset.Now
            };

            await _jobRunService.Create(await GetClaimsPrincipal(), jobRun);

            using (var logContext = LogContext.PushProperty("JobRunId", jobRun.Id))
            {
                _logger.LogInformation("Starting job");

                try
                {
                    await RunInternal();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Critical error in job");

                    jobRun.ErrorMessages = GetAllErrorMessages(ex);
                    jobRun.ErrorStackTrace = GetInnerStackTrack(ex);
                }

                //_logger.LogInformation($"Job finished {jobRun.Id.ToString()}: {_successCount} succeeded, {_failureCount} failed");
                _logger.LogInformation("Job finished: {SuccessCount} succeeded, {FailureCount} failed", _successCount, _failureCount);

                jobRun.SuccessCount = _successCount;
                jobRun.FailureCount = _failureCount;
                jobRun.EndTime = DateTimeOffset.Now;

                await _jobRunService.Update(await GetClaimsPrincipal(), jobRun);
            }
        }

        protected abstract Task RunInternal();

        protected string GetAllErrorMessages(Exception ex)
        {
            List<Exception> exceptions = new List<Exception>();
            while (ex != null)
            {
                exceptions.Add(ex);
                ex = ex.InnerException;
            }

            return string.Join("\n", exceptions.Select(x => x.Message));
        }

        protected string GetInnerStackTrack(Exception ex)
        {
            string returnValue = "";
            while (ex != null)
            {
                returnValue = ex.StackTrace;
                ex = ex.InnerException;
            }
            return returnValue;
        }
    }
}
