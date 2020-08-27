using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Models.DTO.Places;
using MkeTools.Web.Services;

namespace MkeTools.Web.Controllers.Data
{
    public class ParcelController : EntityReadController<Parcel, ParcelDTO, IEntityReadService<Parcel, string>, string>
    {
        public ParcelController(IConfiguration configuration, IMapper mapper, IEntityReadService<Parcel, string> service) : base(configuration, mapper, service)
        {
        }
    }
}
