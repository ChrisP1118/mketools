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
using MkeAlerts.Web.Services.Functional;
using MkeAlerts.Web.Utilities;
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
    public class SendPickupDatesNotifications : Job
    {
        private readonly ILogger<SendPickupDatesNotifications> _logger;
        private readonly IEntityWriteService<PickupDatesSubscription, Guid> _pickupDatesSubscriptionService;

        public SendPickupDatesNotifications(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, ILogger<SendPickupDatesNotifications> logger, IEntityWriteService<PickupDatesSubscription, Guid> pickupDatesSubscriptionService)
            : base(configuration, signInManager, userManager, mailerService)
        {
            _pickupDatesSubscriptionService = pickupDatesSubscriptionService;
            _logger = logger;
        }

        public async Task Run()
        {
            DateTime now = DateTime.Now;

            _logger.LogInformation("Starting job: " + now.ToShortDateString() + " " + now.ToShortTimeString());

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
            try
            {
                string mixedType = type.Substring(0, 1).ToUpper() + type.Substring(1);

                if (!nextPickup.HasValue)
                {
                    _logger.LogWarning($"Next{mixedType}PickupDate is null: {subscription.Id}");
                    return;
                }

                _logger.LogInformation($"Subscription notification for {type} pickup: " + subscription.Id + " (" + subscription.ApplicationUser.Email + ")");

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
                _logger.LogError($"Error sending {type} pickup date notification {subscription.Id}", ex);
            }
        }
    }
}