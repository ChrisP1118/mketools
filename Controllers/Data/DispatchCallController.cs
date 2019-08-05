using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Models.Data.Properties;
using MkeAlerts.Web.Services;
using MkeAlerts.Web.Models.Data.DispatchCalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MkeAlerts.Web.Models.DTO.DispatchCalls;

namespace MkeAlerts.Web.Controllers.Data
{
    public class DispatchCallController : EntityReadController<DispatchCall, DispatchCallDTO, IEntityReadService<DispatchCall, string>, string>
    {
        public DispatchCallController(IConfiguration configuration, IMapper mapper, IEntityReadService<DispatchCall, string> service) : base(configuration, mapper, service)
        {
        }
    }
}
