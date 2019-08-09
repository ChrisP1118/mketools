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
    public class DispatchCallController : EntityReadController<DispatchCall, DispatchCallDTO, IEntityReadService<DispatchCall, string>, string>
    {
        public DispatchCallController(IConfiguration configuration, IMapper mapper, IEntityReadService<DispatchCall, string> service) : base(configuration, mapper, service)
        {
        }
    }
}
