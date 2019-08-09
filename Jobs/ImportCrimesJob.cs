using DotSpatial.Projections;
using GeoAPI.Geometries;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Incidents;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Services;
using MkeAlerts.Web.Utilities;
using NetTopologySuite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;

namespace MkeAlerts.Web.Jobs
{
    public class ImportCrimesJob : ImportXmlJob<Crime>
    {
        protected ProjectionInfo _projectionInfo;

        public ImportCrimesJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<ImportXmlJob<Crime>> logger, IEntityWriteService<Crime, string> writeService) :
            base(configuration, signInManager, userManager, logger, writeService)
        {
            string path = @"M:\My Documents\GitHub\mkealerts\DataSources\parcelbase_mprop_full\parcelbase_mprop_full.shp";
            _projectionInfo = ProjectionInfo.Open(path.Replace(".shp", ".prj"));
        }

        protected override string GetFileName()
        {
            return @"M:\My Documents\GitHub\mkealerts\DataSources\wibr\wibr.xml";
        }

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
            IGeometryFactory geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            Tuple<double, double> projectedCoordinates = GeographicUtilities.ReprojectCoordinates(_projectionInfo, item.RoughX, item.RoughY);
            IPoint projectedPoint = geometryFactory.CreatePoint(new Coordinate(projectedCoordinates.Item1, projectedCoordinates.Item2));

            item.Point = projectedPoint;
        }
    }
}