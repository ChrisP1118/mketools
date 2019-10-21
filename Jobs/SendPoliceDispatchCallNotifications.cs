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
    public class SendPoliceDispatchCallNotifications : Job
    {
        private readonly ILogger<SendPoliceDispatchCallNotifications> _logger;
        private readonly IEntityReadService<PoliceDispatchCall, string> _policeDispatchCallService;
        private readonly IEntityReadService<PoliceDispatchCallType, string> _policeDispatchCallTypeService;
        private readonly IEntityReadService<DispatchCallSubscription, Guid> _dispatchCallSubscriptionService;
        private readonly IMailerService _mailerService;

        public SendPoliceDispatchCallNotifications(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<SendPoliceDispatchCallNotifications> logger, IEntityReadService<PoliceDispatchCall, string> policeDispatchCallService, IEntityReadService<PoliceDispatchCallType, string> policeDispatchCallTypeService, IEntityReadService<DispatchCallSubscription, Guid> dispatchCallSubscriptionService, IMailerService mailerService)
            : base(configuration, signInManager, userManager)
        {
            _policeDispatchCallService = policeDispatchCallService;
            _policeDispatchCallTypeService = policeDispatchCallTypeService;
            _dispatchCallSubscriptionService = dispatchCallSubscriptionService;
            _logger = logger;
            _mailerService = mailerService;
        }

        public async Task Run(string policeDispatchCallId)
        {
            _logger.LogInformation("Starting job: " + policeDispatchCallId);

            ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();
            PoliceDispatchCall policeDispatchCall = await _policeDispatchCallService.GetOne(claimsPrincipal, policeDispatchCallId);
            PoliceDispatchCallType policeDispatchCallType = await _policeDispatchCallTypeService.GetOne(claimsPrincipal, policeDispatchCall.NatureOfCall);

            List<DispatchCallSubscription> dispatchCallSubscriptions = await _dispatchCallSubscriptionService.GetAll(claimsPrincipal, 0, 100000, null, null, null, null, null, null, queryable =>
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
                if (emailAddresses.Contains(dispatchCallSubscription.ApplicationUser.Email))
                {
                    _logger.LogInformation("Skipping duplicate subscription notification for police dispatch call " + policeDispatchCall.GetId() + ": " + dispatchCallSubscription.Id + " (" + dispatchCallSubscription.ApplicationUser.Email + ")");
                    continue;
                }

                _logger.LogInformation("Subscription notification for police dispatch call " + policeDispatchCall.GetId() + ": " + dispatchCallSubscription.Id + " (" + dispatchCallSubscription.ApplicationUser.Email + ")");

                await _mailerService.SendEmail(
                    dispatchCallSubscription.ApplicationUser.Email,
                    "Police Dispatch Call: " + policeDispatchCall.NatureOfCall + " at " + policeDispatchCall.Location,
                    "A new " + policeDispatchCall.NatureOfCall + " police dispatch call was made at " + policeDispatchCall.ReportedDateTime.ToShortTimeString() + " on " + policeDispatchCall.ReportedDateTime.ToShortDateString() + " at " + policeDispatchCall.Location + ".",
                    "<p>A new " + policeDispatchCall.NatureOfCall + " police dispatch call was made at " + policeDispatchCall.ReportedDateTime.ToShortTimeString() + " on " + policeDispatchCall.ReportedDateTime.ToShortDateString() + " at " + policeDispatchCall.Location + ".</p>"
                );

                emailAddresses.Add(dispatchCallSubscription.ApplicationUser.Email);
            }

        }
    }
}