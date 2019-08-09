using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Models.Data.Incidents;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Models.DTO.Incidents;
using MkeAlerts.Web.Models.DTO.Places;
using MkeAlerts.Web.Services;

namespace MkeAlerts.Web.Controllers.Data
{
    public class CrimeController : EntityReadController<Crime, CrimeDTO, IEntityReadService<Crime, string>, string>
    {
        public CrimeController(IConfiguration configuration, IMapper mapper, IEntityReadService<Crime, string> service) : base(configuration, mapper, service)
        {
        }
    }
}
