using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MkeAlerts.Web.Models.DTO.Accounts;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Models.DTO.Places;
using MkeAlerts.Web.Models.Data.Incidents;
using MkeAlerts.Web.Models.DTO.Incidents;

namespace MkeAlerts.Web.Models.Data
{
    public class DataModelsProfile : Profile
    {
        public DataModelsProfile()
        {
            CreateMap<RegisterDTO, ApplicationUser>();

            CreateMap<ApplicationUser, ApplicationUserDTO>();

            CreateMap<Property, PropertyDTO>().ReverseMap();
            CreateMap<Parcel, ParcelDTO>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<Street, StreetDTO>().ReverseMap();
            CreateMap<DispatchCall, DispatchCallDTO>().ReverseMap();
            CreateMap<Crime, CrimeDTO>().ReverseMap();
        }
    }
}
