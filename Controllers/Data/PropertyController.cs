using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Models.DTO.Places;
using MkeAlerts.Web.Services;

namespace MkeAlerts.Web.Controllers.Data
{
    public class PropertyController : EntityReadController<Property, PropertyDTO, IEntityReadService<Property, string>, string>
    {
        public PropertyController(IConfiguration configuration, IMapper mapper, IEntityReadService<Property, string> service) : base(configuration, mapper, service)
        {
        }
    }
}
