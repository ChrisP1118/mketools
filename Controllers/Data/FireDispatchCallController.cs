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
    public class FireDispatchCallController : EntityReadController<FireDispatchCall, FireDispatchCallDTO, IEntityReadService<FireDispatchCall, string>, string>
    {
        public FireDispatchCallController(IConfiguration configuration, IMapper mapper, IEntityReadService<FireDispatchCall, string> service) : base(configuration, mapper, service)
        {
        }
    }
}
