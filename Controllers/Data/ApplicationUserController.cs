using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.DTO.Accounts;
using MkeAlerts.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Controllers.Data
{
    public class ApplicationUserController : EntityReadController<ApplicationUser, ApplicationUserDTO, IEntityReadService<ApplicationUser, Guid>, Guid>
    {
        public ApplicationUserController(IConfiguration configuration, IMapper mapper, IEntityReadService<ApplicationUser, Guid> service) : base(configuration, mapper, service)
        {
        }
    }
}
