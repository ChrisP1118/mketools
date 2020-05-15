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
using MkeAlerts.Web.Models.Data.Subscriptions;
using MkeAlerts.Web.Models.DTO.Subscriptions;
using MkeAlerts.Web.Models.DTO;
using MkeAlerts.Web.Models.Internal;
using MkeAlerts.Web.Models.DTO.Geocoding;
using MkeAlerts.Web.Models.DTO.PickupDates;
using NetTopologySuite.Geometries;

namespace MkeAlerts.Web.Models.Data
{
    public class DataModelsProfile : Profile
    {
        public DataModelsProfile()
        {
            CreateMap<RegisterDTO, ApplicationUser>();

            CreateMap<ApplicationUser, ApplicationUserDTO>();

            CreateMap<Parcel, ParcelDTO>().ReverseMap();
            CreateMap<CommonParcel, CommonParcelDTO>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<Street, StreetDTO>().ReverseMap();
            CreateMap<Property, PropertyDTO>().ReverseMap();
            CreateMap<CurrentProperty, CurrentPropertyDTO>().ReverseMap();
            CreateMap<PoliceDispatchCall, PoliceDispatchCallDTO>().ReverseMap();
            CreateMap<PoliceDispatchCallType, PoliceDispatchCallTypeDTO>().ReverseMap();
            CreateMap<FireDispatchCall, FireDispatchCallDTO>().ReverseMap();
            CreateMap<FireDispatchCallType, FireDispatchCallTypeDTO>().ReverseMap();
            CreateMap<Crime, CrimeDTO>().ReverseMap();

            CreateMap<DispatchCallSubscription, DispatchCallSubscriptionDTO>().ReverseMap();
            CreateMap<PickupDatesSubscription, PickupDatesSubscriptionDTO>().ReverseMap();

            CreateMap<Point, PointDTO>().ReverseMap();

            CreateMap<GeocodeResults, GeocodeResultsDTO>().ReverseMap();
            CreateMap<ReverseGeocodeResults, ReverseGeocodeResultsDTO>().ReverseMap();
            CreateMap<PickupDatesResults, PickupDatesResultDTO>().ReverseMap();
        }
    }
}
