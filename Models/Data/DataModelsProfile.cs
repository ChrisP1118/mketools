using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MkeAlerts.Web.Models.DTO.Accounts;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Properties;
using MkeAlerts.Web.Models.DTO.Properties;
using MkeAlerts.Web.Models.Data.DispatchCalls;
using MkeAlerts.Web.Models.DTO.DispatchCalls;

namespace MkeAlerts.Web.Models.Data
{
    public class DataModelsProfile : Profile
    {
        public DataModelsProfile()
        {
            CreateMap<RegisterDTO, ApplicationUser>();

            CreateMap<ApplicationUser, ApplicationUserDTO>();

            CreateMap<Property, PropertyDTO>().ReverseMap();
            CreateMap<DispatchCall, DispatchCallDTO>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<Location, LocationDTO>().ReverseMap();
        }
    }
}
