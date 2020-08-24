using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Services;
using MkeAlerts.Web.Models.Data.Incidents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MkeAlerts.Web.Models.DTO.Incidents;
using MkeAlerts.Web.Models.Data.HistoricPhotos;
using MkeAlerts.Web.Models.DTO.HistoricPhotos;

namespace MkeAlerts.Web.Controllers.Data
{
    public class HistoricPhotoLocationController : EntityReadController<HistoricPhotoLocation, HistoricPhotoLocationDTO, IEntityReadService<HistoricPhotoLocation, Guid>, Guid>
    {
        public HistoricPhotoLocationController(IConfiguration configuration, IMapper mapper, IEntityReadService<HistoricPhotoLocation, Guid> service) : base(configuration, mapper, service)
        {
        }
    }
}
