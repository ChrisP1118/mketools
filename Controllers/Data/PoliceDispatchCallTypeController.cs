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
    public class PoliceDispatchCallTypeController : EntityReadController<PoliceDispatchCallType, PoliceDispatchCallTypeDTO, IEntityReadService<PoliceDispatchCallType, string>, string>
    {
        public PoliceDispatchCallTypeController(IConfiguration configuration, IMapper mapper, IEntityReadService<PoliceDispatchCallType, string> service) : base(configuration, mapper, service)
        {
        }
    }
}
