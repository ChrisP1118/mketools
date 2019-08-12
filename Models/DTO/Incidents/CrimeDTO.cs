﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Incidents
{
    public class CrimeDTO
    {
        public string IncidentNum { get; set; }

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
    }
}