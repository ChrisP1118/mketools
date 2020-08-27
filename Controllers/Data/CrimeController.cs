using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeTools.Web.Models.Data.Incidents;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Models.DTO.Incidents;
using MkeTools.Web.Models.DTO.Places;
using MkeTools.Web.Services;

namespace MkeTools.Web.Controllers.Data
{
    public class CrimeController : EntityReadController<Crime, CrimeDTO, IEntityReadService<Crime, string>, string>
    {
        public CrimeController(IConfiguration configuration, IMapper mapper, IEntityReadService<Crime, string> service) : base(configuration, mapper, service)
        {
        }
    }
}
