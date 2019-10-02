using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Models.Data.Subscriptions;
using MkeAlerts.Web.Models.DTO.Places;
using MkeAlerts.Web.Models.DTO.Subscriptions;
using MkeAlerts.Web.Services;
using System;

namespace MkeAlerts.Web.Controllers.Data
{
    [Authorize]
    public class DispatchCallSubscriptionController : EntityWriteController<DispatchCallSubscription, DispatchCallSubscriptionDTO, IEntityWriteService<DispatchCallSubscription, Guid>, Guid>
    {
        public DispatchCallSubscriptionController(IConfiguration configuration, IMapper mapper, IEntityWriteService<DispatchCallSubscription, Guid> service) : base(configuration, mapper, service)
        {
        }
    }
}
