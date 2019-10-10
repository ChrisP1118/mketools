using GeoAPI.Geometries;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Models;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Models.Internal;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Location = MkeAlerts.Web.Models.Data.Places.Parcel;

namespace MkeAlerts.Web.Services.Functional
{
    public interface IGeocodingService
    {
        Task<GeocodeResults> Geocode(string value);
        Task<ReverseGeocodeResults> ReverseGeocode(double latitude, double longitude);
    }

    public class GeocodingService : IGeocodingService
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly ILogger<GeocodingService> _logger;

        protected Dictionary<string, string> _suffixes = new Dictionary<string, string>() {
            { "TR", "TR" },
            { "AV", "AV" },
            { "RD", "RD" },
            { "CR", "CR" },
            { "WA", "WA" },
            { "CT", "CT" },
            { "LA", "LA" },
            { "PK", "PK" },
            { "ST", "ST" },
            { "DR", "DR" },
            { "PL", "PL" },
            { "BL", "BL" },
            { "AVE", "AV" },
            { "BLVD", "BL" },
            { "CIR", "CR" },
            { "PKWY", "PK" },
            { "TER", "TR" },
            { "WAY", "WA" }
        };

        public GeocodingService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, ILogger<GeocodingService> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
        }

        private class GeocodeRequest
        {
            public GeocodeRequest(string rawValue, string value)
            {
                RawValue = rawValue;
                Value = value;
                Results = new GeocodeResults();
            }

            public string RawValue { get; } = "";
            public string Value { get; set; } = "";

            public GeocodeResults Results { get; set; }
        }

        private class AddressGeocodeRequest : GeocodeRequest
        {
            public AddressGeocodeRequest(string rawValue, string value) : base(rawValue, value)
            {
            }

            public int HouseNumber { get; set; }
            public string Direction { get; set; } = "";
            public string Street { get; set; } = "";
            public string StreetType { get; set; } = "";
        }

        private class IntersectionGeocodeRequest : GeocodeRequest
        {
            public IntersectionGeocodeRequest(string rawValue, string value) : base(rawValue, value)
            {
            }

            public GeocodeRequestStreet[] Streets = new GeocodeRequestStreet[2];
        }

        private class GeocodeRequestStreet
        {
            public string Direction { get; set; } = "";
            public string Street { get; set; } = "";
            public string StreetType { get; set; } = "";
        }

        private GeocodeResults GetNoGeometryResult()
        {
            return new GeocodeResults
            {
                Geometry = null,
                Accuracy = GeometryAccuracy.NoGeometry,
                Source = GeometrySource.NoGeometry
            };
        }

        public async Task<GeocodeResults> Geocode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return GetNoGeometryResult();

            try
            {
                string modValue = value.Trim();
                if (modValue.EndsWith(",MKE"))
                    modValue = modValue.Substring(0, modValue.Length - 4);

                modValue = modValue.ToUpper();

                if (modValue.Contains("/"))
                    return await GeocodeIntersection(new IntersectionGeocodeRequest(value, modValue));
                else
                    return await GeocodeAddress(new AddressGeocodeRequest(value, modValue));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error geocoding value: " + value);
                return GetNoGeometryResult();
            }
        }

        private async Task<GeocodeResults> GeocodeIntersection(IntersectionGeocodeRequest request)
        {
            string[] streets = request.Value.Split("/");
            if (streets.Length != 2)
            {
                _logger.LogWarning("More than two streets found: " + request.RawValue);
                return GetNoGeometryResult();
            }

            for (int i = 0; i < 2; ++i)
            {
                request.Streets[i] = new GeocodeRequestStreet();

                streets[i] = streets[i].Trim();

                string[] parts = streets[i].Split(" ");
                if (parts.Length < 3)
                {
                    _logger.LogWarning("Unexpected street format: " + request.RawValue);
                    return GetNoGeometryResult();
                }

                request.Streets[i].Direction = parts[0];

                if (_suffixes.Keys.Contains(parts[parts.Length - 1]))
                    request.Streets[i].StreetType = _suffixes[parts[parts.Length - 1]];

                request.Streets[i].Street = string.Join(' ', parts, 1, parts.Length - (request.Streets[i].StreetType == "" ? 1 : 2));

                if (request.Streets[i].Direction.EndsWith("."))
                    request.Streets[i].Direction = request.Streets[i].Direction.Substring(0, request.Streets[i].Direction.Length - 1);
                if (request.Streets[i].StreetType.EndsWith("."))
                    request.Streets[i].Street = request.Streets[i].StreetType.Substring(0, request.Streets[i].Direction.Length - 1);
            }

            List<Street> streets0 = await GetStreets(request.Streets[0]);
            List<Street> streets1 = await GetStreets(request.Streets[1]);

            if (streets0.Count() == 0)
            {
                _logger.LogWarning("No streets found for " + request.Streets[0].Street + ": " + request.RawValue);
                return GetNoGeometryResult();
            }

            if (streets1.Count() == 0)
            {
                _logger.LogWarning("No streets found for " + request.Streets[1].Street + ": " + request.RawValue);
                return GetNoGeometryResult();
            }

            foreach (Street street0 in streets0)
            {
                foreach (Street street1 in streets1)
                {
                    IGeometry intersection = street0.Outline.Intersection(street1.Outline);
                    if (intersection != null && !intersection.IsEmpty)
                    {
                        request.Results.Geometry = intersection;
                        request.Results.Accuracy = GeometryAccuracy.High;
                        request.Results.Source = GeometrySource.StreetIntersection;

                        return request.Results;
                    }
                }
            }

            _logger.LogWarning("No intersection found for : " + request.RawValue);
            return GetNoGeometryResult();
        }

        private async Task<List<Street>> GetStreets(GeocodeRequestStreet request)
        {
            return await _dbContext.Streets
                .Where(s => s.DIR == request.Direction)
                .Where(s => s.STREET == request.Street)
                .Where(s => s.STTYPE == request.StreetType || request.StreetType == "")
                .ToListAsync();
        }

        private async Task<GeocodeResults> GeocodeAddress(AddressGeocodeRequest request)
        {
            string[] parts = request.Value.Split(" ");

            if (parts.Length < 3)
            {
                _logger.LogWarning("Too few parts for value: " + request.RawValue);
                return GetNoGeometryResult();
            }

            request.StreetType = "";
            if (_suffixes.Keys.Contains(parts[parts.Length - 1]))
                request.StreetType = _suffixes[parts[parts.Length - 1]];

            string houseNumberString = parts[0];

            // Sometimes a block is indicated, like "2300-BLK N 54TH ST,MKE"
            houseNumberString = houseNumberString.Replace("-BLK", "");
            request.HouseNumber = int.Parse(houseNumberString);
            request.Direction = parts[1];
            request.Street = string.Join(' ', parts, 2, parts.Length - (request.StreetType == "" ? 2 : 3));

            if (request.Direction.EndsWith("."))
                request.Direction = request.Direction.Substring(0, request.Direction.Length - 1);
            if (request.StreetType.EndsWith("."))
                request.Street = request.StreetType.Substring(0, request.Direction.Length - 1);

            Address address = await GetAddress(request);

            if (address != null)
            {
                request.Results.Geometry = address.Property.Parcel.Outline;
                request.Results.Accuracy = GeometryAccuracy.High;
                request.Results.Source = GeometrySource.AddressAndLocation;

                return request.Results;
            }

            address = await GetNearbyAddress(request);

            if (address != null)
            {
                request.Results.Geometry = address.Property.Parcel.Outline;
                request.Results.Accuracy = GeometryAccuracy.Medium;
                request.Results.Source = GeometrySource.AddressBlock;

                return request.Results;
            }

            _logger.LogWarning("Unable to find address/property/location: " + request.Value);

            return GetNoGeometryResult();
        }

        //private async Task<Location> GetLocation(Address address)
        //{
        //    return await _dbContext.Locations
        //        .Where(l => l.TAXKEY == address.TAXKEY)
        //        .FirstOrDefaultAsync();
        //}

        private async Task<Address> GetAddress(AddressGeocodeRequest request)
        {
            return await _dbContext.Addresses
                .Include(a => a.Property)
                .Include(a => a.Property.Parcel)
                .Where(a => a.HSE_NBR == request.HouseNumber)
                .Where(a => a.DIR == request.Direction)
                .Where(a => a.STREET == request.Street)
                .Where(a => a.STTYPE == request.StreetType || request.StreetType == "")
                .Where(a => a.Property != null)
                .Where(a => a.Property.Parcel != null)
                .FirstOrDefaultAsync();
        }

        private async Task<Address> GetNearbyAddress(AddressGeocodeRequest request)
        {
            int houseNumberLow = (int)Math.Floor((double)request.HouseNumber / 100d) * 100;
            int houseNumberHigh = (int)Math.Ceiling((double)request.HouseNumber / 100d) * 100;

            // If houseNumber is a multiple of 100, the low and high are the same number
            if (houseNumberHigh == request.HouseNumber)
                houseNumberHigh += 100;

            var addresses = await _dbContext.Addresses
                .Include(a => a.Property)
                .Include(a => a.Property.Parcel)
                .Where(a => a.HSE_NBR >= houseNumberLow)
                .Where(a => a.HSE_NBR < houseNumberHigh)
                .Where(a => a.DIR == request.Direction)
                .Where(a => a.STREET == request.Street)
                .Where(a => a.STTYPE == request.StreetType || request.StreetType == "")
                .Where(a => a.Property != null)
                .Where(a => a.Property.Parcel != null)
                .ToListAsync();

            if (addresses.Count() == 0)
                return null;

            return addresses.OrderBy(x => Math.Abs(x.HSE_NBR - request.HouseNumber)).First();
        }

        public async Task<ReverseGeocodeResults> ReverseGeocode(double latitude, double longitude)
        {
            // The returned values are off by a few blocks. Sigh.

            Point location = new Point(longitude, latitude)
            {
                SRID = 4326
            };

            double northBound = Math.Ceiling(latitude * 100) / 100;
            double southBound = Math.Floor(latitude * 100) / 100;
            double westBound = Math.Floor(longitude * 100) / 100;
            double eastBound = Math.Ceiling(longitude * 100) / 100;

            northBound += 0.01;
            southBound -= 0.01;
            westBound -= 0.01;
            eastBound += 0.01;

            Parcel parcel = await _dbContext.Parcels
                .Include(p => p.Property)
                .Where(p => p.Property != null)
                .Where(x =>
                    (x.MinLat <= northBound && x.MaxLat >= northBound) ||
                    (x.MinLat <= southBound && x.MaxLat >= southBound) ||
                    (x.MinLat >= northBound && x.MaxLat <= southBound) ||
                    (x.MinLat >= southBound && x.MaxLat <= northBound))
                .Where(x =>
                    (x.MinLng <= westBound && x.MaxLng >= westBound) ||
                    (x.MinLng <= eastBound && x.MaxLng >= eastBound) ||
                    (x.MinLng >= westBound && x.MaxLng <= eastBound) ||
                    (x.MinLng >= eastBound && x.MaxLng <= westBound))
                .OrderBy(p => p.Outline.Distance(location))
                .FirstOrDefaultAsync();

            if (parcel == null)
                return null;

            return new ReverseGeocodeResults()
            {
                Property = parcel.Property,
                Distance = parcel.Outline.Distance(location)
            };
        }
    }
}
