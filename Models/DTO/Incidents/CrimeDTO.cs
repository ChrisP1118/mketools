using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Incidents
{
    public class CrimeDTO
    {
        public string IncidentNum { get; set; }

        [JsonConverter(typeof(GeometryConverter))]
        public Geometry Point { get; set; }

        public DateTime ReportedDateTime { get; set; }
        public decimal ReportedYear { get; set; }
        public decimal ReportedMonth { get; set; }

        public string Location { get; set; }
        public string WeaponUsed { get; set; }

        public decimal ALD { get; set; }
        public decimal NSP { get; set; }
        public decimal POLICE { get; set; }
        public decimal TRACT { get; set; }
        public decimal WARD { get; set; }
        public decimal ZIP { get; set; }
        public double RoughX { get; set; }
        public double RoughY { get; set; }
        public int Arson { get; set; }
        public int AssaultOffense { get; set; }
        public int Burglary { get; set; }
        public int CriminalDamage { get; set; }
        public int Homicide { get; set; }
        public int LockedVehicle { get; set; }
        public int Robbery { get; set; }
        public int SexOffense { get; set; }
        public int Theft { get; set; }
        public int VehicleTheft { get; set; }

        public string CrimeTypes
        {
            get
            {
                List<string> typesOfCrime = new List<string>();

                if (Arson == 1)
                    typesOfCrime.Add("Arson");
                if (AssaultOffense == 1)
                    typesOfCrime.Add("Assault Offense");
                if (Burglary == 1)
                    typesOfCrime.Add("Burglary");
                if (CriminalDamage == 1)
                    typesOfCrime.Add("Criminal Damage");
                if (Homicide == 1)
                    typesOfCrime.Add("Homicide");
                if (LockedVehicle == 1)
                    typesOfCrime.Add("Locked Vehicle");
                if (Robbery == 1)
                    typesOfCrime.Add("Robbery");
                if (SexOffense == 1)
                    typesOfCrime.Add("Sex Offense");
                if (Theft == 1)
                    typesOfCrime.Add("Theft");
                if (VehicleTheft == 1)
                    typesOfCrime.Add("Vehicle Theft");

                return string.Join(", ", typesOfCrime);
            }
        }
    }
}
