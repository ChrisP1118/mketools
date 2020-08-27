using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Models.DTO.Places;
using MkeTools.Web.Services;

namespace MkeTools.Web.Controllers.Data
{
    public class AddressController : EntityReadController<Address, AddressDTO, IEntityReadService<Address, int>, int>
    {
        public AddressController(IConfiguration configuration, IMapper mapper, IEntityReadService<Address, int> service) : base(configuration, mapper, service)
        {
        }
    }
}
