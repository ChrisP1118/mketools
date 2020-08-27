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
    public class FireDispatchCallTypeController : EntityReadController<FireDispatchCallType, FireDispatchCallTypeDTO, IEntityReadService<FireDispatchCallType, string>, string>
    {
        public FireDispatchCallTypeController(IConfiguration configuration, IMapper mapper, IEntityReadService<FireDispatchCallType, string> service) : base(configuration, mapper, service)
        {
        }
    }
}
