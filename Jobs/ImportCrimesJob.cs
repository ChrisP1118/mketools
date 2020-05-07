using DotSpatial.Projections;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Incidents;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Models.Internal;
using MkeAlerts.Web.Services;
using MkeAlerts.Web.Services.Data.Interfaces;
using MkeAlerts.Web.Services.Functional;
using MkeAlerts.Web.Utilities;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;

namespace MkeAlerts.Web.Jobs
{
    public class ImportCrimesJob : ImportXmlJob<Crime>
    {
        protected ProjectionInfo _projectionInfo;
        protected readonly IGeocodingService _geocodingService;

        public ImportCrimesJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<ImportCrimesJob> logger, IEntityWriteService<Crime, string> writeService, IGeocodingService geocodingService) :
            base(configuration, signInManager, userManager, mailerService, jobRunService, logger, writeService)
        {
            string path = Path.Combine(AppContext.BaseDirectory, @"GeoData\parcelbase_mprop_full.prj");
            _projectionInfo = ProjectionInfo.Open(path);
            _geocodingService = geocodingService;
        }

        protected override string PackageName => "wibr";
        protected override string PackageFormat => "XML";

        protected override bool UseBulkInsert => false;

        protected override void ProcessElement(Crime item, string elementName, string elementValue)
        {
            switch (elementName)
            {
                case "IncidentNum": item.IncidentNum = elementValue; break;
                case "ReportedDateTime": item.ReportedDateTime = DateTime.Parse(elementValue); break;
                case "ReportedYear": item.ReportedYear = decimal.Parse(elementValue); break;
                case "ReportedMonth": item.ReportedMonth = decimal.Parse(elementValue); break;
                case "Location": item.Location = elementValue; break;
                case "WeaponUsed": item.WeaponUsed = elementValue; break;
                case "ALD": item.ALD = decimal.Parse(elementValue); break;
                case "NSP": item.NSP = decimal.Parse(elementValue); break;
                case "POLICE": item.POLICE = decimal.Parse(elementValue); break;
                case "TRACT": item.TRACT = decimal.Parse(elementValue); break;
                case "WARD": item.WARD = decimal.Parse(elementValue); break;
                case "ZIP": item.ZIP = decimal.Parse(elementValue); break;
                case "RoughX": item.RoughX = double.Parse(elementValue); break;
                case "RoughY": item.RoughY = double.Parse(elementValue); break;
                case "Arson": item.Arson = int.Parse(elementValue); break;
                case "AssaultOffense": item.AssaultOffense = int.Parse(elementValue); break;
                case "Burglary": item.Burglary = int.Parse(elementValue); break;
                case "CriminalDamage": item.CriminalDamage = int.Parse(elementValue); break;
                case "Homicide": item.Homicide = int.Parse(elementValue); break;
                case "LockedVehicle": item.LockedVehicle = int.Parse(elementValue); break;
                case "Robbery": item.Robbery = int.Parse(elementValue); break;
                case "SexOffense": item.SexOffense = int.Parse(elementValue); break;
                case "Theft": item.Theft = int.Parse(elementValue); break;
                case "VehicleTheft": item.VehicleTheft = int.Parse(elementValue); break;
            }
        }

        protected override async Task BeforeSaveElement(Crime item)
        {
            // Attempt to geocode the location
            GeocodeResults geocodeResults = await _geocodingService.Geocode(item.Location);

            if (geocodeResults.Accuracy == Models.GeometryAccuracy.High || geocodeResults.Accuracy == Models.GeometryAccuracy.Medium)
            {
                if (geocodeResults.Geometry is Point point)
                    item.Point = point;
                else if (geocodeResults.Geometry is Polygon polygon)
                    item.Point = polygon.Centroid;
                else if (geocodeResults.Geometry is MultiPolygon multiPolygon)
                    item.Point = multiPolygon.Centroid;
            }

            // If geocoding fails, fall back on the imported coordinates (which are only accurate to the block level, not address)
            if (item.Point == null)
            {
                GeometryFactory geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

                Tuple<double, double> projectedCoordinates = GeographicUtilities.ReprojectCoordinates(_projectionInfo, item.RoughX, item.RoughY);
                Point projectedPoint = geometryFactory.CreatePoint(new Coordinate(projectedCoordinates.Item1, projectedCoordinates.Item2));

                item.Point = projectedPoint;
            }

            GeographicUtilities.SetBounds(item, item.Point);
        }
    }
}