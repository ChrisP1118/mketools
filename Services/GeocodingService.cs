using GeoAPI.Geometries;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Models;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Properties;
using MkeAlerts.Web.Models.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;
using Location = MkeAlerts.Web.Models.Data.Properties.Location;

namespace MkeAlerts.Web.Services
{
    public interface IGeocodingService
    {
        Task<GeocodeResults> Geocode(string value);
    }

    public class GeocodingService : IGeocodingService
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly ILogger<GeocodingService> _logger;

        protected string[] _suffixes = new string[12] {
            "TR",
            "AV",
            "RD",
            "CR",
            "WA",
            "CT",
            "LA",
            "PK",
            "ST",
            "DR",
            "PL",
            "BL" };

        public GeocodingService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, ILogger<GeocodingService> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
        }

        private class GeocodeRequest
        {
            public string Value { get; set; }
            public string StreetType { get; set; }
            public int HouseNumber { get; set; }
            public string Direction { get; set; }
            public string Street { get; set; }

            public GeocodeResults Results { get; set; }
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
                GeocodeRequest request = new GeocodeRequest()
                {
                    Value = value,
                    Results = new GeocodeResults()
                };

                return await GeocodeValue(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error geocoding value: " + value);
                return GetNoGeometryResult();
            }
        }

        private async Task<GeocodeResults> GeocodeValue(GeocodeRequest request)
        {
            request.Value = request.Value.Trim();
            if (request.Value.EndsWith(",MKE"))
                request.Value = request.Value.Substring(0, request.Value.Length - 4);

            if (request.Value.Contains(" / "))
                return await GeocodeIntersection(request);
            else
                return await GeocodeAddress(request);
        }

        private async Task<GeocodeResults> GeocodeIntersection(GeocodeRequest request)
        {
            _logger.LogWarning("Intersection not found: " + request.Value);
            return GetNoGeometryResult();
        }

        private async Task<GeocodeResults> GeocodeAddress(GeocodeRequest request)
        {
            string[] parts = request.Value.Split(" ");

            if (parts.Length < 3)
            {
                _logger.LogWarning("Too few parts for value: " + request.Value);
                return GetNoGeometryResult();
            }

            request.StreetType = "";
            if (_suffixes.Contains(parts[parts.Length - 1]))
                request.StreetType = parts[parts.Length - 1];
            request.HouseNumber = int.Parse(parts[0]);
            request.Direction = parts[1];
            request.Street = string.Join(' ', parts, 2, parts.Length - (request.StreetType == "" ? 2 : 3));

            if (request.Direction.EndsWith("."))
                request.Direction = request.Direction.Substring(0, request.Direction.Length - 1);
            if (request.StreetType.EndsWith("."))
                request.Street = request.StreetType.Substring(0, request.Direction.Length - 1);

            Address address = await GetAddress(request);

            if (address != null)
            {
                Location location = await GetLocation(address);

                if (location != null)
                {
                    request.Results.Geometry = location.Outline;
                    request.Results.Accuracy = GeometryAccuracy.High;
                    request.Results.Source = GeometrySource.AddressAndLocation;

                    return request.Results;
                }

                _logger.LogInformation("Unable to find location for address: Value: " + request.Value + ", Address.TAXKEY: " + address.TAXKEY);
            }

            address = await GetNearbyAddress(request);

            if (address != null)
            {
                Location location = await GetLocation(address);

                if (location != null)
                {
                    request.Results.Geometry = location.Outline;
                    request.Results.Accuracy = GeometryAccuracy.Medium;
                    request.Results.Source = GeometrySource.AddressBlock;

                    return request.Results;
                }

                _logger.LogInformation("Unable to find location for nearby address: Value: " + request.Value + ", Address.TAXKEY: " + address.TAXKEY);
            }

            _logger.LogWarning("Unable to find address: Value: " + request.Value);

            return GetNoGeometryResult();
        }

        private async Task<Location> GetLocation(Address address)
        {
            return await _dbContext.Locations
                .Where(l => l.TAXKEY == address.TAXKEY)
                .FirstOrDefaultAsync();
        }

        private async Task<Address> GetAddress(GeocodeRequest request)
        {
            return await _dbContext.Addresses
                .Where(a => a.HSE_NBR == request.HouseNumber)
                .Where(a => a.DIR == request.Direction)
                .Where(a => a.STREET == request.Street)
                .Where(a => a.STTYPE == request.StreetType)
                .FirstOrDefaultAsync();
        }

        private async Task<Address> GetNearbyAddress(GeocodeRequest request)
        {
            // TODO: Let's have this only include addresses that have a location

            int houseNumberLow = (int)Math.Floor((double)request.HouseNumber / 100d) * 100;
            int houseNumberHigh = (int)Math.Ceiling((double)request.HouseNumber / 100d) * 100;

            var addresses = await _dbContext.Addresses
                .Where(a => a.HSE_NBR >= houseNumberLow)
                .Where(a => a.HSE_NBR < houseNumberHigh)
                .Where(a => a.DIR == request.Direction)
                .Where(a => a.STREET == request.Street)
                .Where(a => a.STTYPE == request.StreetType)
                .ToListAsync();

            if (addresses.Count() == 0)
                return null;

            return addresses.OrderBy(x => Math.Abs(x.HSE_NBR - request.HouseNumber)).First();
        }
    }
}
