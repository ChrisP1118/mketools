using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Models.DTO.Places;
using MkeAlerts.Web.Services;

namespace MkeAlerts.Web.Controllers.Data
{
    public class LocationController : EntityReadController<Location, LocationDTO, IEntityReadService<Location, string>, string>
    {
        public LocationController(IConfiguration configuration, IMapper mapper, IEntityReadService<Location, string> service) : base(configuration, mapper, service)
        {
        }
    }
}
