using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Models.DTO.Places;
using MkeTools.Web.Services;

namespace MkeTools.Web.Controllers.Data
{
    public class StreetController : EntityReadController<Street, StreetDTO, IEntityReadService<Street, int>, int>
    {
        public StreetController(IConfiguration configuration, IMapper mapper, IEntityReadService<Street, int> service) : base(configuration, mapper, service)
        {
        }
    }
}
