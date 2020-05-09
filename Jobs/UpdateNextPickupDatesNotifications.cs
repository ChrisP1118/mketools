using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Models.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Incidents;
using MkeAlerts.Web.Models.Data.Subscriptions;
using MkeAlerts.Web.Models.Internal;
using MkeAlerts.Web.Services;
using MkeAlerts.Web.Services.Data.Interfaces;
using MkeAlerts.Web.Services.Functional;
using MkeAlerts.Web.Utilities;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Jobs
{
    public class UpdateNextPickupDatesNotifications : LoggedJob
    {
        private readonly IPickupDatesService _pickupDatesService;
        private readonly IEntityWriteService<PickupDatesSubscription, Guid> _pickupDatesSubscriptionService;

        public UpdateNextPickupDatesNotifications(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<UpdateNextPickupDatesNotifications> logger, IPickupDatesService pickupDatesService, IEntityWriteService<PickupDatesSubscription, Guid> pickupDatesSubscriptionService)
            : base(configuration, signInManager, userManager, mailerService, jobRunService, logger)
        {
            _pickupDatesService = pickupDatesService;
            _pickupDatesSubscriptionService = pickupDatesSubscriptionService;
        }

        protected override async Task RunInternal()
        {
            DateTime now = DateTime.Now;

            _logger.LogInformation("Now: {Now}", now);

            ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

            List<PickupDatesSubscription> pickupDatesSubscriptions = await _pickupDatesSubscriptionService.GetAll(claimsPrincipal, 0, 100000, null, null, null, null, null, null, null, true, false, queryable => { return queryable; });

            foreach (PickupDatesSubscription pickupDatesSubscription in pickupDatesSubscriptions)
            {
                using (var logContext = LogContext.PushProperty("SubscriptionId", pickupDatesSubscription.Id))
                {
                    try
                    {
                        _logger.LogInformation($"Updating next pickups");

                        PickupDatesResults pickupDatesResults = await _pickupDatesService.GetPickupDates(pickupDatesSubscription.LADDR, pickupDatesSubscription.SDIR, pickupDatesSubscription.SNAME, pickupDatesSubscription.STYPE);

                        pickupDatesSubscription.NextGarbagePickupDate = pickupDatesResults.NextGarbagePickupDate;
                        pickupDatesSubscription.NextRecyclingPickupDate = pickupDatesResults.NextRecyclingPickupDate;

                        pickupDatesSubscription.NextGarbagePickupNotification = null;
                        pickupDatesSubscription.NextRecyclingPickupNotification = null;

                        if (pickupDatesResults.NextGarbagePickupDate.HasValue)
                        {
                            DateTime notificationDate = pickupDatesResults.NextGarbagePickupDate.Value.AddHours(pickupDatesSubscription.HoursBefore);
                            if (notificationDate >= now)
                                pickupDatesSubscription.NextGarbagePickupNotification = notificationDate;
                        }

                        if (pickupDatesResults.NextRecyclingPickupDate.HasValue)
                        {
                            DateTime notificationDate = pickupDatesResults.NextRecyclingPickupDate.Value.AddHours(pickupDatesSubscription.HoursBefore);
                            if (notificationDate >= now)
                                pickupDatesSubscription.NextRecyclingPickupNotification = notificationDate;
                        }

                        await _pickupDatesSubscriptionService.Update(await GetClaimsPrincipal(), pickupDatesSubscription);

                        ++_successCount;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error updating pickups");

                        ++_failureCount;
                    }
                }
            }
        }
    }
}