using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Services;
using MkeTools.Web.Models.Data.Incidents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MkeTools.Web.Models.DTO.Incidents;
using MkeTools.Web.Models.Data.HistoricPhotos;
using MkeTools.Web.Models.DTO.HistoricPhotos;

namespace MkeTools.Web.Controllers.Data
{
    public class HistoricPhotoController : EntityReadController<HistoricPhoto, HistoricPhotoDTO, IEntityReadService<HistoricPhoto, string>, string>
    {
        public HistoricPhotoController(IConfiguration configuration, IMapper mapper, IEntityReadService<HistoricPhoto, string> service) : base(configuration, mapper, service)
        {
        }
    }
}
