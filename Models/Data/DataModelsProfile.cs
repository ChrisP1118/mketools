using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MkeTools.Web.Models.DTO.Accounts;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Models.DTO.Places;
using MkeTools.Web.Models.Data.Incidents;
using MkeTools.Web.Models.DTO.Incidents;
using MkeTools.Web.Models.Data.Subscriptions;
using MkeTools.Web.Models.DTO.Subscriptions;
using MkeTools.Web.Models.DTO;
using MkeTools.Web.Models.Internal;
using MkeTools.Web.Models.DTO.Geocoding;
using MkeTools.Web.Models.DTO.PickupDates;
using NetTopologySuite.Geometries;
using MkeTools.Web.Models.Data.HistoricPhotos;
using MkeTools.Web.Models.DTO.HistoricPhotos;

namespace MkeTools.Web.Models.Data
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
            CreateMap<HistoricPhoto, HistoricPhotoDTO>().ReverseMap();
            CreateMap<HistoricPhotoLocation, HistoricPhotoLocationDTO>().ReverseMap();

            CreateMap<DispatchCallSubscription, DispatchCallSubscriptionDTO>().ReverseMap();
            CreateMap<PickupDatesSubscription, PickupDatesSubscriptionDTO>().ReverseMap();

            CreateMap<Point, PointDTO>().ReverseMap();

            CreateMap<GeocodeResults, GeocodeResultsDTO>().ReverseMap();
            CreateMap<ReverseGeocodeResults, ReverseGeocodeResultsDTO>().ReverseMap();
            CreateMap<PickupDatesResults, PickupDatesResultDTO>().ReverseMap();
        }
    }
}
