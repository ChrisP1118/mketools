using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MkeTools.Web.Data;
using MkeTools.Web.Models;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Models.Internal;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Functional
{
    public class GeocodingService : IGeocodingService
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly ILogger<GeocodingService> _logger;

        protected Dictionary<string, string> _suffixes = new Dictionary<string, string>() {
            { "AV", "AVE" },
            { "AVE", "AVE" },
            { "BL", "BLVD" },
            { "BLVD", "BLVD" },
            { "CR", "CIR" },
            { "CIR", "CIR" },
            { "CT", "CT" },
            { "DR", "DR" },
            { "FWY", "FWY" },
            { "LA", "LN" },
            { "LN", "LN" },
            { "LOOP", "LOOP" },
            { "PASS", "PASS" },
            { "PK", "PKWY" },
            { "PKWY", "PKWY" },
            { "PL", "PL" },
            { "RD", "RD" },
            { "RDGE", "RDGE" },
            { "ROW", "ROW" },
            { "RUN", "RUN" },
            { "SQ", "SQ" },
            { "ST", "ST" },
            { "TER", "TER" },
            { "TR", "TER" },
            { "TRL", "TRL" },
            { "WA", "WAY" },
            { "WAY", "WAY" }
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

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            GeocodeResults geocodeResults = null;

            try
            {
                string modValue = value.Trim();
                if (modValue.LastIndexOf(",") > 0)
                    modValue = modValue.Substring(0, modValue.LastIndexOf(","));
                //if (modValue.EndsWith(",MKE"))
                //    modValue = modValue.Substring(0, modValue.Length - 4);

                modValue = modValue.ToUpper();

                if (modValue.Contains("/"))
                    geocodeResults = await GeocodeIntersection(new IntersectionGeocodeRequest(value, modValue));
                else
                    geocodeResults = await GeocodeAddress(new AddressGeocodeRequest(value, modValue));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error geocoding value: {GeocodeValue}", value);
                geocodeResults = GetNoGeometryResult();
            }

            stopwatch.Stop();
            _logger.LogInformation("Geocoded {GeocodeValue} from {GeocodeSource} to {GeocodeAccuracy} in {GeocodeTime}ms", value, geocodeResults.Source, geocodeResults.Accuracy, stopwatch.ElapsedMilliseconds);

            return geocodeResults;
        }

        private async Task<GeocodeResults> GeocodeIntersection(IntersectionGeocodeRequest request)
        {
            string[] streets = request.Value.Split("/");
            if (streets.Length != 2)
            {
                _logger.LogWarning("More than two streets found: {GeocodeValue}", request.RawValue);
                return GetNoGeometryResult();
            }

            for (int i = 0; i < 2; ++i)
            {
                request.Streets[i] = new GeocodeRequestStreet();

                streets[i] = streets[i].Trim();

                string[] parts = streets[i].Split(" ");
                if (parts.Length < 3)
                {
                    _logger.LogWarning("Unexpected street format: {GeocodeValue}", request.RawValue);
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

            var result = await _dbContext.Streets
                .Join(
                    _dbContext.Streets,
                    s => s.CLINEID,
                    s => s.CLINEID,
                    (s1, s2) => new { Street1 = s1, Street2 = s2 }
                )
                .Where(x => x.Street1.Outline.Intersects(x.Street2.Outline))
                .FirstOrDefaultAsync();

            if (result != null)
            {
                request.Results.Geometry = result.Street1.Outline.Intersection(result.Street2.Outline);
                request.Results.Accuracy = GeometryAccuracy.High;
                request.Results.Source = GeometrySource.StreetIntersection;

                return request.Results;
            }

            _logger.LogWarning("No intersection found for: {GeocodeValue}", request.RawValue);
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
                _logger.LogWarning("Too few parts for value: {GeocodeValue}", request.RawValue);
                return GetNoGeometryResult();
            }

            request.StreetType = "";
            if (_suffixes.Keys.Contains(parts[parts.Length - 1]))
                request.StreetType = _suffixes[parts[parts.Length - 1]];

            string houseNumberString = parts[0];

            // Sometimes a block is indicated, like "2300-BLK N 54TH ST,MKE"
            if (houseNumberString.IndexOf("-") > 0)
                houseNumberString = houseNumberString.Substring(0, houseNumberString.IndexOf("-"));
            //houseNumberString = houseNumberString.Replace("-BLK", "");
            //houseNumberString = houseNumberString.Replace("-BLOCK", "");
            
            // Strip out the non-numeric characters
            request.HouseNumber = int.Parse(new String(houseNumberString.Where(c => char.IsDigit(c)).ToArray()));
            request.Direction = parts[1];
            request.Street = string.Join(' ', parts, 2, parts.Length - (request.StreetType == "" ? 2 : 3));

            if (request.Direction.EndsWith("."))
                request.Direction = request.Direction.Substring(0, request.Direction.Length - 1);
            if (request.StreetType.EndsWith("."))
                request.Street = request.StreetType.Substring(0, request.Direction.Length - 1);

            // Query for an exact address
            Address address = await GetAddress(request);
            if (address != null)
            {
                request.Results.Geometry = address.Point;
                request.Results.Accuracy = GeometryAccuracy.High;
                request.Results.Source = GeometrySource.ExactAddress;

                return request.Results;
            }

            // Query for an exact parcel
            Parcel parcel = await GetParcel(request);
            if (parcel != null)
            {
                request.Results.Geometry = parcel.CommonParcel.Outline.Centroid;
                request.Results.Accuracy = GeometryAccuracy.High;
                request.Results.Source = GeometrySource.ExactParcel;

                return request.Results;
            }

            // Query for nearby addresses
            address = await GetNearbyAddress(request);
            if (address != null)
            {
                request.Results.Geometry = address.Point;
                request.Results.Accuracy = GeometryAccuracy.Medium;
                request.Results.Source = GeometrySource.NearbyAddress;

                return request.Results;
            }

            // Query for nearby parcels
            parcel = await GetNearbyParcel(request);
            if (parcel != null)
            {
                request.Results.Geometry = parcel.CommonParcel.Outline.Centroid;
                request.Results.Accuracy = GeometryAccuracy.Medium;
                request.Results.Source = GeometrySource.NearbyParcel;

                return request.Results;
            }

            // Query for nearby streets
            Street street = await GetNearbyStreet(request);
            if (street != null)
            {
                request.Results.Geometry = street.Outline.Centroid;
                request.Results.Accuracy = GeometryAccuracy.Medium;
                request.Results.Source = GeometrySource.NearbyStreet;

                return request.Results;
            }

            _logger.LogWarning("Unable to find address/property/location: {GeocodeValue}", request.Value);

            return GetNoGeometryResult();
        }

        private async Task<Address> GetAddress(AddressGeocodeRequest request)
        {
            return await _dbContext.Addresses
                .Where(a => a.HouseNumber == request.HouseNumber)
                .Where(a => a.DIR == request.Direction)
                .Where(a => a.STREET == request.Street)
                .Where(a => a.STTYPE == request.StreetType || request.StreetType == "")
                .Where(a => a.Point != null)
                .FirstOrDefaultAsync();
        }

        private async Task<Parcel> GetParcel(AddressGeocodeRequest request)
        {
            return await _dbContext.Parcels
                .Include(x => x.CommonParcel)
                .Where(x => x.HouseNumber == request.HouseNumber)
                .Where(x => x.STREETDIR == request.Direction)
                .Where(x => x.STREETNAME == request.Street)
                .Where(x => x.STREETTYPE == request.StreetType)
                .Where(x => x.CommonParcel != null)
                .FirstOrDefaultAsync();
        }

        private async Task<Address> GetNearbyAddress(AddressGeocodeRequest request)
        {
            int houseNumberLow = (int)Math.Floor((double)request.HouseNumber / 100d) * 100;
            int houseNumberHigh = (int)Math.Ceiling((double)request.HouseNumber / 100d) * 100;

            houseNumberHigh = houseNumberHigh + 100;
            houseNumberLow = houseNumberLow - 100;

            if (houseNumberLow < 1)
                houseNumberLow = 1;

            var addresses = await _dbContext.Addresses
                .Where(a => a.HouseNumber >= houseNumberLow)
                .Where(a => a.HouseNumber < houseNumberHigh)
                .Where(a => a.DIR == request.Direction)
                .Where(a => a.STREET == request.Street)
                .Where(a => a.STTYPE == request.StreetType || request.StreetType == "")
                .Where(a => a.Point != null)
                .ToListAsync();

            if (addresses.Count() == 0)
                return null;

            return addresses.OrderBy(x => Math.Abs(x.HouseNumber - request.HouseNumber)).First();
        }

        private async Task<Parcel> GetNearbyParcel(AddressGeocodeRequest request)
        {
            int houseNumberLow = (int)Math.Floor((double)request.HouseNumber / 100d) * 100;
            int houseNumberHigh = (int)Math.Ceiling((double)request.HouseNumber / 100d) * 100;

            // If houseNumber is a multiple of 100, the low and high are the same number
            if (houseNumberHigh == request.HouseNumber)
                houseNumberHigh += 100;

            var parcels = await _dbContext.Parcels
                .Include(a => a.CommonParcel)
                .Where(a => a.HouseNumber >= houseNumberLow)
                .Where(a => a.HouseNumber < houseNumberHigh)
                .Where(a => a.STREETDIR == request.Direction)
                .Where(a => a.STREETNAME == request.Street)
                .Where(a => a.STREETTYPE == request.StreetType || request.StreetType == "")
                .Where(a => a.CommonParcel != null)
                .ToListAsync();

            if (parcels.Count() == 0)
                return null;

            return parcels.OrderBy(x => Math.Abs(x.HouseNumber - request.HouseNumber)).First();
        }

        private async Task<Street> GetNearbyStreet(AddressGeocodeRequest request)
        {
            if (request.HouseNumber % 2 == 0)
            {
                // Even number = left
                return await _dbContext.Streets
                    .Where(a => a.DIR == request.Direction)
                    .Where(a => a.STREET == request.Street)
                    .Where(a => a.STTYPE == request.StreetType)
                    .Where(a => a.LeftNumberLow <= request.HouseNumber && a.LeftNumberHigh >= request.HouseNumber)
                    .FirstOrDefaultAsync();
            }
            else
            {
                // Odd number = right
                return await _dbContext.Streets
                    .Where(a => a.DIR == request.Direction)
                    .Where(a => a.STREET == request.Street)
                    .Where(a => a.STTYPE == request.StreetType)
                    .Where(a => a.RightNumberLow <= request.HouseNumber && a.RightNumberHigh >= request.HouseNumber)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<ReverseGeocodeResults> ReverseGeocode(double latitude, double longitude)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Point location = new Point(longitude, latitude)
            {
                SRID = 4326
            };

            double northBound = latitude;
            double southBound = latitude;
            double westBound = longitude;
            double eastBound = longitude;

            CommonParcel commonParcel = null;

            for (int attempt = 0; attempt < 3; ++attempt)
            {
                northBound += 0.0003;
                southBound -= 0.0003;
                westBound -= 0.0003;
                eastBound += 0.0003;

                commonParcel = await _dbContext.CommonParcels
                    .Include(p => p.Parcels)
                    .Where(p => p.Parcels != null)
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

                if (commonParcel != null)
                    break;
            }

            stopwatch.Stop();
            _logger.LogInformation("Reverse geocoded {GeocodeLatitude},{GeocodeLongitude} to {CommonParcelId} in {GeocodeTime}ms", latitude, longitude, commonParcel?.GetId().ToString() ?? "(none)", stopwatch.ElapsedMilliseconds);

            if (commonParcel == null)
                return null;

            return new ReverseGeocodeResults()
            {
                CommonParcel = commonParcel,
                Distance = commonParcel.Outline.Distance(location)
            };
        }
    }
}
