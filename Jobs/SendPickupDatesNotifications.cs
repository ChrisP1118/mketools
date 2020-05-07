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
using System.Web;

namespace MkeAlerts.Web.Jobs
{
    public class SendPickupDatesNotifications : LoggedJob
    {
        private readonly IEntityWriteService<PickupDatesSubscription, Guid> _pickupDatesSubscriptionService;

        public SendPickupDatesNotifications(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<SendPickupDatesNotifications> logger, IEntityWriteService<PickupDatesSubscription, Guid> pickupDatesSubscriptionService)
            : base(configuration, signInManager, userManager, mailerService, jobRunService, logger)
        {
            _pickupDatesSubscriptionService = pickupDatesSubscriptionService;
        }

        protected override async Task RunInternal()
        {
            DateTime now = DateTime.Now;

            _logger.LogInformation("Now: {Now}", now);

            ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

            List<PickupDatesSubscription> garbageSubscriptions = await _pickupDatesSubscriptionService.GetAll(claimsPrincipal, 0, 100000, null, null, null, null, null, null, null, true, false, queryable =>
            {
                return queryable
                    .Include(x => x.ApplicationUser)
                    .Where(x => x.NextGarbagePickupNotification <= now);
            });

            List<PickupDatesSubscription> recyclingSubscriptions = await _pickupDatesSubscriptionService.GetAll(claimsPrincipal, 0, 100000, null, null, null, null, null, null, null, true, false, queryable =>
            {
                return queryable
                    .Include(x => x.ApplicationUser)
                    .Where(x => x.NextRecyclingPickupNotification <= now);
            });

            foreach (PickupDatesSubscription garbageSubscription in garbageSubscriptions)
                await SendNotification(garbageSubscription, "garbage", garbageSubscription.NextGarbagePickupDate, s => s.NextGarbagePickupNotification = null);

            foreach (PickupDatesSubscription recyclingSubscription in recyclingSubscriptions)
                await SendNotification(recyclingSubscription, "recycling", recyclingSubscription.NextGarbagePickupDate, s => s.NextRecyclingPickupNotification = null);
        }

        private async Task SendNotification(PickupDatesSubscription subscription, string type, DateTime? nextPickup, Action<PickupDatesSubscription> updateAction)
        {
            using (var logContext1 = LogContext.PushProperty("SubscriptionId", subscription.Id))
            using (var logContext2 = LogContext.PushProperty("SubscriptionType", "PickupDates"))
            using (var logContext3 = LogContext.PushProperty("SubscriptionEmail", subscription.ApplicationUser?.Email ?? "(null)"))
            using (var logContext4 = LogContext.PushProperty("PickupDateType", type))
            {
                try
                {
                    string mixedType = type.Substring(0, 1).ToUpper() + type.Substring(1);

                    if (!nextPickup.HasValue)
                    {
                        _logger.LogWarning("{SubscriptionAction} notification", "Skipping (No pickup date)");
                        return;
                    }

                    _logger.LogInformation("{SubscriptionAction} notification", "Sending");

                    string hash = EncryptionUtilities.GetHash(subscription.Id.ToString() + ":" + subscription.ApplicationUserId.ToString(), _configuration["HashKey"]);
                    string unsubscribeUrl = string.Format(_configuration["PickupDatesUnsubscribeUrl"], subscription.Id, subscription.ApplicationUserId, HttpUtility.UrlEncode(hash));
                    string address = $"{subscription.LADDR} {subscription.SDIR} {subscription.SNAME} {subscription.STYPE}";
                    string day = nextPickup.Value.ToString("dddd, MMMM d");

                    string text = $@"The next {type} pickup day for {address} is {day}.\r\n\r\n" +
                        $@"You are receiving this message because you receive notifications for {type} pickup days at {address}. To stop receiving these email notifications, go to: {unsubscribeUrl}.";

                    string html = $@"<p>The next {type} pickup day for {address} is {day}.</p>" +
                        $@"<hr />" +
                        $@"<p style=""font-size: 80%"">You are receiving this message because you receive notifications for {type} pickup days at {address}. <a href=""{unsubscribeUrl}"">Unsubscribe</a></p>";

                    await _mailerService.SendEmail(
                        subscription.ApplicationUser.Email,
                        $"{mixedType} Pickup Day: {day}",
                        text,
                        html
                    );

                    updateAction(subscription);

                    await _pickupDatesSubscriptionService.Update(await GetClaimsPrincipal(), subscription);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending notification");
                }
            }
        }
    }
}