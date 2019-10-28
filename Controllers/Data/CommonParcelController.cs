using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Models.DTO.Places;
using MkeAlerts.Web.Services;
using System;

namespace MkeAlerts.Web.Controllers.Data
{
    public class CommonParcelController : EntityReadController<CommonParcel, CommonParcelDTO, IEntityReadService<CommonParcel, Guid>, Guid>
    {
        public CommonParcelController(IConfiguration configuration, IMapper mapper, IEntityReadService<CommonParcel, Guid> service) : base(configuration, mapper, service)
        {
        }
    }
}
