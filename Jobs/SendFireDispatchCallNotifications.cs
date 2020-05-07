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
using Serilog;
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
    public class SendFireDispatchCallNotifications : Job
    {
        private readonly ILogger<SendFireDispatchCallNotifications> _logger;
        private readonly IEntityReadService<FireDispatchCall, string> _fireDispatchCallService;
        private readonly IEntityReadService<FireDispatchCallType, string> _fireDispatchCallTypeService;
        private readonly IEntityReadService<DispatchCallSubscription, Guid> _dispatchCallSubscriptionService;

        public SendFireDispatchCallNotifications(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, ILogger<SendFireDispatchCallNotifications> logger, IEntityReadService<FireDispatchCall, string> fireDispatchCallService, IEntityReadService<FireDispatchCallType, string> fireDispatchCallTypeService, IEntityReadService<DispatchCallSubscription, Guid> dispatchCallSubscriptionService)
            : base(configuration, signInManager, userManager, mailerService)
        {
            _fireDispatchCallService = fireDispatchCallService;
            _fireDispatchCallTypeService = fireDispatchCallTypeService;
            _dispatchCallSubscriptionService = dispatchCallSubscriptionService;
            _logger = logger;
        }

        public async Task Run(string fireDispatchCallId)
        {
            using (var logContext = LogContext.PushProperty("FireDispatchCallId", fireDispatchCallId))
            {
                _logger.LogInformation("Starting job");

                ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();
                FireDispatchCall fireDispatchCall = await _fireDispatchCallService.GetOne(claimsPrincipal, fireDispatchCallId, null);
                FireDispatchCallType fireDispatchCallType = await _fireDispatchCallTypeService.GetOne(claimsPrincipal, fireDispatchCall.NatureOfCall, null);

                List<DispatchCallSubscription> dispatchCallSubscriptions = await _dispatchCallSubscriptionService.GetAll(claimsPrincipal, 0, 100000, null, null, null, null, null, null, null, true, true, queryable =>
                {
                    // IPoint.Distance is confusing. When this gets translated into a SQL query, it ends up using STDistance which interprets in meters on our geography type. However, if you run it in managed
                    // code, it seems to be interpreting it as a geometry instead of geography, so it's the distance in the coordinate system, which is kind of useless.

                    queryable = queryable
                        .Include(x => x.ApplicationUser)
                        .Where(x => x.Point.Distance(fireDispatchCall.Geometry) < x.Distance * 0.3048); // Distance is stored in feet, which we convert here to meters

                    if (fireDispatchCallType.IsMajor)
                        queryable = queryable.Where(x => x.DispatchCallType.HasFlag(DispatchCallType.JustMajorFireDispatchCall));
                    else
                        queryable = queryable.Where(x => x.DispatchCallType.HasFlag(DispatchCallType.JustMinorFireDispatchCall));

                    return queryable;
                });

                // There may be more than one subscription that covers a given dispatch call, so we keep track of the ones we've already sent so we don't send duplicates
                List<string> emailAddresses = new List<string>();

                foreach (DispatchCallSubscription dispatchCallSubscription in dispatchCallSubscriptions)
                {
                    using (var logContext1 = LogContext.PushProperty("SubscriptionId", dispatchCallSubscription.Id))
                    using (var logContext2 = LogContext.PushProperty("SubscriptionType", "FireDispatchCall"))
                    using (var logContext3 = LogContext.PushProperty("SubscriptionEmail", dispatchCallSubscription.ApplicationUser?.Email ?? "(null)"))
                    {
                        try
                        {
                            if (emailAddresses.Contains(dispatchCallSubscription.ApplicationUser.Email))
                            {
                                _logger.LogInformation("{SubscriptionAction} notification to {SubscriptionEmail} for {SubscriptionType}: {SubscriptionId}", "Skipping (Duplicate)");
                                continue;
                            }

                            _logger.LogInformation("{SubscriptionAction} notification to {SubscriptionEmail} for {SubscriptionType}: {SubscriptionId}", "Sending");

                            string hash = EncryptionUtilities.GetHash(dispatchCallSubscription.Id.ToString() + ":" + dispatchCallSubscription.ApplicationUserId.ToString(), _configuration["HashKey"]);
                            string unsubscribeUrl = string.Format(_configuration["DispatchCallUnsubscribeUrl"], dispatchCallSubscription.Id, dispatchCallSubscription.ApplicationUserId, HttpUtility.UrlEncode(hash));
                            string detailsUrl = string.Format(_configuration["FireDispatchCallUrl"], fireDispatchCall.CFS);

                            string text = $@"A new {fireDispatchCall.NatureOfCall} fire dispatch call was made at {fireDispatchCall.ReportedDateTime.ToShortTimeString()} on {fireDispatchCall.ReportedDateTime.ToShortDateString() + " at " + fireDispatchCall.Address}.\r\n\r\n" +
                                $@"More details are at: {detailsUrl}" + "\r\n\r\n" +
                                $@"You are receiving this message because you receive notifications for dispatch calls near {dispatchCallSubscription.HOUSE_NR} {dispatchCallSubscription.SDIR} {dispatchCallSubscription.STREET} {dispatchCallSubscription.STTYPE}. To stop receiving these email notifications, go to: {unsubscribeUrl}.";

                            string html = $@"<p>A new <b>{fireDispatchCall.NatureOfCall}</b> fire dispatch call was made at {fireDispatchCall.ReportedDateTime.ToShortTimeString()} on {fireDispatchCall.ReportedDateTime.ToShortDateString() + " at <b>" + fireDispatchCall.Address}</b>.</p>" +
                                $@"<p><a href=""{detailsUrl}"">View Details</a></p>" +
                                $@"<hr />" +
                                $@"<p style=""font-size: 80%"">You are receiving this message because you receive notifications for dispatch calls near {dispatchCallSubscription.HOUSE_NR} {dispatchCallSubscription.SDIR} {dispatchCallSubscription.STREET} {dispatchCallSubscription.STTYPE}. <a href=""{unsubscribeUrl}"">Unsubscribe</a><p>";

                            await _mailerService.SendEmail(
                                dispatchCallSubscription.ApplicationUser.Email,
                                "Fire Dispatch Call: " + fireDispatchCall.NatureOfCall + " at " + fireDispatchCall.Address,
                                text,
                                html
                            );

                            emailAddresses.Add(dispatchCallSubscription.ApplicationUser.Email);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("Error sending notification to {SubscriptionEmail} for {SubscriptionType}: {SubscriptionId}", ex);
                        }
                    }
                }
            }
        }
    }
}