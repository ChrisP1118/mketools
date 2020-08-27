using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Services;
using MkeTools.Web.Models.Data.Incidents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MkeTools.Web.Models.DTO.Incidents;

namespace MkeTools.Web.Controllers.Data
{
    public class PoliceDispatchCallController : EntityReadController<PoliceDispatchCall, PoliceDispatchCallDTO, IEntityReadService<PoliceDispatchCall, string>, string>
    {
        public PoliceDispatchCallController(IConfiguration configuration, IMapper mapper, IEntityReadService<PoliceDispatchCall, string> service) : base(configuration, mapper, service)
        {
        }
    }
}
