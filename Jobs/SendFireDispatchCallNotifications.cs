using GeoAPI.Geometries;
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
            _logger.LogInformation("Starting job: " + fireDispatchCallId);

            ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();
            FireDispatchCall fireDispatchCall = await _fireDispatchCallService.GetOne(claimsPrincipal, fireDispatchCallId);
            FireDispatchCallType fireDispatchCallType = await _fireDispatchCallTypeService.GetOne(claimsPrincipal, fireDispatchCall.NatureOfCall);

            List<DispatchCallSubscription> dispatchCallSubscriptions = await _dispatchCallSubscriptionService.GetAll(claimsPrincipal, 0, 100000, null, null, null, null, null, null, queryable =>
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
                if (emailAddresses.Contains(dispatchCallSubscription.ApplicationUser.Email))
                {
                    _logger.LogInformation("Skipping duplicate subscription notification for fire dispatch call " + fireDispatchCall.GetId() + ": " + dispatchCallSubscription.Id + " (" + dispatchCallSubscription.ApplicationUser.Email + ")");
                    continue;
                }

                _logger.LogInformation("Subscription notification for fire dispatch call " + fireDispatchCall.GetId() + ": " + dispatchCallSubscription.Id + " (" + dispatchCallSubscription.ApplicationUser.Email + ")");

                await _mailerService.SendEmail(
                    dispatchCallSubscription.ApplicationUser.Email,
                    "Fire Dispatch Call: " + fireDispatchCall.NatureOfCall + " at " + fireDispatchCall.Address,
                    "A new " + fireDispatchCall.NatureOfCall + " fire dispatch call was made at " + fireDispatchCall.ReportedDateTime.ToShortTimeString() + " on " + fireDispatchCall.ReportedDateTime.ToShortDateString() + " at " + fireDispatchCall.Address + ".",
                    "<p>A new " + fireDispatchCall.NatureOfCall + " fire dispatch call was made at " + fireDispatchCall.ReportedDateTime.ToShortTimeString() + " on " + fireDispatchCall.ReportedDateTime.ToShortDateString() + " at " + fireDispatchCall.Address + ".</p>"
                );

                emailAddresses.Add(dispatchCallSubscription.ApplicationUser.Email);
            }

        }
    }
}