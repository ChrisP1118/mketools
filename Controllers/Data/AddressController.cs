using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Models.DTO.Places;
using MkeAlerts.Web.Services;

namespace MkeAlerts.Web.Controllers.Data
{
    public class AddressController : EntityReadController<Address, AddressDTO, IEntityReadService<Address, int>, int>
    {
        public AddressController(IConfiguration configuration, IMapper mapper, IEntityReadService<Address, int> service) : base(configuration, mapper, service)
        {
        }
    }
}
