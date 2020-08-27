using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.DTO.Accounts;
using MkeTools.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Controllers.Data
{
    public class ApplicationUserController : EntityReadController<ApplicationUser, ApplicationUserDTO, IEntityReadService<ApplicationUser, Guid>, Guid>
    {
        public ApplicationUserController(IConfiguration configuration, IMapper mapper, IEntityReadService<ApplicationUser, Guid> service) : base(configuration, mapper, service)
        {
        }
    }
}
