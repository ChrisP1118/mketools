using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Models.DTO.Places;
using MkeAlerts.Web.Services;
using System;

namespace MkeAlerts.Web.Controllers.Data
{
    public class PropertyController : EntityReadController<Property, PropertyDTO, IEntityReadService<Property, Guid>, Guid>
    {
        public PropertyController(IConfiguration configuration, IMapper mapper, IEntityReadService<Property, Guid> service) : base(configuration, mapper, service)
        {
        }
    }
}
