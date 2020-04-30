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
    public class SendPoliceDispatchCallNotifications : Job
    {
        private readonly ILogger<SendPoliceDispatchCallNotifications> _logger;
        private readonly IEntityReadService<PoliceDispatchCall, string> _policeDispatchCallService;
        private readonly IEntityReadService<PoliceDispatchCallType, string> _policeDispatchCallTypeService;
        private readonly IEntityReadService<DispatchCallSubscription, Guid> _dispatchCallSubscriptionService;

        public SendPoliceDispatchCallNotifications(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, ILogger<SendPoliceDispatchCallNotifications> logger, IEntityReadService<PoliceDispatchCall, string> policeDispatchCallService, IEntityReadService<PoliceDispatchCallType, string> policeDispatchCallTypeService, IEntityReadService<DispatchCallSubscription, Guid> dispatchCallSubscriptionService)
            : base(configuration, signInManager, userManager, mailerService)
        {
            _policeDispatchCallService = policeDispatchCallService;
            _policeDispatchCallTypeService = policeDispatchCallTypeService;
            _dispatchCallSubscriptionService = dispatchCallSubscriptionService;
            _logger = logger;
        }

        public async Task Run(string policeDispatchCallId)
        {
            _logger.LogInformation("Starting job: " + policeDispatchCallId);

            ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();
            PoliceDispatchCall policeDispatchCall = await _policeDispatchCallService.GetOne(claimsPrincipal, policeDispatchCallId, null);
            PoliceDispatchCallType policeDispatchCallType = await _policeDispatchCallTypeService.GetOne(claimsPrincipal, policeDispatchCall.NatureOfCall, null);

            List<DispatchCallSubscription> dispatchCallSubscriptions = await _dispatchCallSubscriptionService.GetAll(claimsPrincipal, 0, 100000, null, null, null, null, null, null, null, true, true, queryable =>
            {
                // IPoint.Distance is confusing. When this gets translated into a SQL query, it ends up using STDistance which interprets in meters on our geography type. However, if you run it in managed
                // code, it seems to be interpreting it as a geometry instead of geography, so it's the distance in the coordinate system, which is kind of useless.

                queryable = queryable
                    .Include(x => x.ApplicationUser)
                    .Where(x => x.Point.Distance(policeDispatchCall.Geometry) < x.Distance * 0.3048); // Distance is stored in feet, which we convert here to meters

                if (policeDispatchCallType == null)
                    queryable = queryable.Where(x => x.DispatchCallType.HasFlag(DispatchCallType.JustNonCrimePoliceDispatchCall));
                else if (policeDispatchCallType.IsMajor)
                    queryable = queryable.Where(x => x.DispatchCallType.HasFlag(DispatchCallType.JustMajorPoliceDispatchCall));
                else if (policeDispatchCallType.IsMinor)
                    queryable = queryable.Where(x => x.DispatchCallType.HasFlag(DispatchCallType.JustMinorPoliceDispatchCall));
                else
                    queryable = queryable.Where(x => x.DispatchCallType.HasFlag(DispatchCallType.JustNonCrimePoliceDispatchCall));

                return queryable;
            });

            // There may be more than one subscription that covers a given dispatch call, so we keep track of the ones we've already sent so we don't send duplicates
            List<string> emailAddresses = new List<string>();

            foreach (DispatchCallSubscription dispatchCallSubscription in dispatchCallSubscriptions)
            {
                try
                {
                    if (emailAddresses.Contains(dispatchCallSubscription.ApplicationUser.Email))
                    {
                        _logger.LogInformation("Skipping duplicate subscription notification for police dispatch call " + policeDispatchCall.GetId() + ": " + dispatchCallSubscription.Id + " (" + dispatchCallSubscription.ApplicationUser.Email + ")");
                        continue;
                    }

                    _logger.LogInformation("Subscription notification for police dispatch call " + policeDispatchCall.GetId() + ": " + dispatchCallSubscription.Id + " (" + dispatchCallSubscription.ApplicationUser.Email + ")");

                    string hash = EncryptionUtilities.GetHash(dispatchCallSubscription.Id.ToString() + ":" + dispatchCallSubscription.ApplicationUserId.ToString(), _configuration["HashKey"]);
                    string unsubscribeUrl = string.Format(_configuration["DispatchCallUnsubscribeUrl"], dispatchCallSubscription.Id, dispatchCallSubscription.ApplicationUserId, HttpUtility.UrlEncode(hash));
                    string detailsUrl = string.Format(_configuration["PoliceDispatchCallUrl"], policeDispatchCall.CallNumber);

                    string text = $@"A new {policeDispatchCall.NatureOfCall} police dispatch call was made at {policeDispatchCall.ReportedDateTime.ToShortTimeString()} on {policeDispatchCall.ReportedDateTime.ToShortDateString() + " at " + policeDispatchCall.Location}.\r\n\r\n" +
                        $@"More details are at: {detailsUrl}" + "\r\n\r\n" +
                        $@"You are receiving this message because you receive notifications for dispatch calls near {dispatchCallSubscription.HOUSE_NR} {dispatchCallSubscription.SDIR} {dispatchCallSubscription.STREET} {dispatchCallSubscription.STTYPE}. To stop receiving these email notifications, go to: {unsubscribeUrl}.";

                    string html = $@"<p>A new <b>{policeDispatchCall.NatureOfCall}</b> police dispatch call was made at {policeDispatchCall.ReportedDateTime.ToShortTimeString()} on {policeDispatchCall.ReportedDateTime.ToShortDateString() + " at <b>" + policeDispatchCall.Location}</b>.</p>" +
                        $@"<p><a href=""{detailsUrl}"">View Details</a></p>" +
                        $@"<hr />" +
                        $@"<p style=""font-size: 80%"">You are receiving this message because you receive notifications for dispatch calls near {dispatchCallSubscription.HOUSE_NR} {dispatchCallSubscription.SDIR} {dispatchCallSubscription.STREET} {dispatchCallSubscription.STTYPE}. <a href=""{unsubscribeUrl}"">Unsubscribe</a><p>";

                    await _mailerService.SendEmail(
                        dispatchCallSubscription.ApplicationUser.Email,
                        "Police Dispatch Call: " + policeDispatchCall.NatureOfCall + " at " + policeDispatchCall.Location,
                        text,
                        html
                    );

                    emailAddresses.Add(dispatchCallSubscription.ApplicationUser.Email);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error sending police dispatch call notification {dispatchCallSubscription.Id}", ex);
                }
            }

        }
    }
}