using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Models.DTO.Places;
using MkeTools.Web.Services;
using System;

namespace MkeTools.Web.Controllers.Data
{
    public class CommonParcelController : EntityReadController<CommonParcel, CommonParcelDTO, IEntityReadService<CommonParcel, int>, int>
    {
        public CommonParcelController(IConfiguration configuration, IMapper mapper, IEntityReadService<CommonParcel, int> service) : base(configuration, mapper, service)
        {
        }
    }
}
