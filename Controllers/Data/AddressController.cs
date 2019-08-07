using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Models.Data.Properties;
using MkeAlerts.Web.Models.DTO.Properties;
using MkeAlerts.Web.Services;

namespace MkeAlerts.Web.Controllers.Data
{
    public class AddressController : EntityReadController<Address, AddressDTO, IEntityReadService<Address, string>, string>
    {
        public AddressController(IConfiguration configuration, IMapper mapper, IEntityReadService<Address, string> service) : base(configuration, mapper, service)
        {
        }
    }
}
