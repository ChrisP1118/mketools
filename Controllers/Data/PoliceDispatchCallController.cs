using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Services;
using MkeAlerts.Web.Models.Data.Incidents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MkeAlerts.Web.Models.DTO.Incidents;

namespace MkeAlerts.Web.Controllers.Data
{
    public class PoliceDispatchCallController : EntityReadController<PoliceDispatchCall, PoliceDispatchCallDTO, IEntityReadService<PoliceDispatchCall, string>, string>
    {
        public PoliceDispatchCallController(IConfiguration configuration, IMapper mapper, IEntityReadService<PoliceDispatchCall, string> service) : base(configuration, mapper, service)
        {
        }
    }
}
