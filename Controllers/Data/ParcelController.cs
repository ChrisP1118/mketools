using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Models.DTO.Places;
using MkeAlerts.Web.Services;

namespace MkeAlerts.Web.Controllers.Data
{
    public class ParcelController : EntityReadController<Parcel, ParcelDTO, IEntityReadService<Parcel, string>, string>
    {
        public ParcelController(IConfiguration configuration, IMapper mapper, IEntityReadService<Parcel, string> service) : base(configuration, mapper, service)
        {
        }
    }
}
