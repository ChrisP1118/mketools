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
using System.Net;

namespace MkeTools.Web.Controllers.Data
{
    public class PoliceDispatchCallTypeController : EntityReadController<PoliceDispatchCallType, PoliceDispatchCallTypeDTO, IEntityReadService<PoliceDispatchCallType, string>, string>
    {
        public PoliceDispatchCallTypeController(IConfiguration configuration, IMapper mapper, IEntityReadService<PoliceDispatchCallType, string> service) : base(configuration, mapper, service)
        {
        }

        protected override string GetOneId(string id)
        {
            return WebUtility.UrlDecode(id);
        }
    }
}
