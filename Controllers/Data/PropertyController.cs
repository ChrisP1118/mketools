using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Models.DTO.Places;
using MkeTools.Web.Services;
using System;

namespace MkeTools.Web.Controllers.Data
{
    public class PropertyController : EntityReadController<Property, PropertyDTO, IEntityReadService<Property, Guid>, Guid>
    {
        public PropertyController(IConfiguration configuration, IMapper mapper, IEntityReadService<Property, Guid> service) : base(configuration, mapper, service)
        {
        }
    }
}
