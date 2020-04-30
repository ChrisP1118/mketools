using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Places
{
    public class ParcelDTO
    {
        [MaxLength(10)]
        public string Taxkey { get; set; }

        public int FK_Parcel { get; set; }
        public int FK_LandUse { get; set; }
        public int FK_Naics { get; set; }
        public int FK_Histori { get; set; }
        public string FK_Zoning { get; set; }
        public string ZoningCFN { get; set; }
        public int ComDiv { get; set; }
        public int Source { get; set; }
        public DateTime RecordDate { get; set; }
        public string Comments { get; set; }
        public string CondoName { get; set; }
        public string CondoType { get; set; }
        public string CondoUnitT { get; set; }
        public DateTime UpdatedDat { get; set; }
        public DateTime ActivatedD { get; set; }
        public int InactiveFl { get; set; }
        public string DistrictNa { get; set; }
        public string Neighborho { get; set; }
        public string PlatPage { get; set; }
        public int StreetNumb { get; set; }
        public int AlternateS { get; set; }
        public string StreetNu_1 { get; set; }
        public string StreetDire { get; set; }
        public string StreetName { get; set; }
        public string STTYPE { get; set; }
        public string LandUse { get; set; }
        public string PropertySt { get; set; }
        public string PrimaryJur { get; set; }
        public string PreviousYe { get; set; }
        public string Previous_1 { get; set; }
        public string CurrentYea { get; set; }
        public string TotalLandV { get; set; }
        public string TotalYardI { get; set; }
        public string TotalAsses { get; set; }
        public string TotalLandE { get; set; }
        public string TotalYar_1 { get; set; }
        public string TotalAss_1 { get; set; }
        public string PrevNonExe { get; set; }
        public string PreviousEx { get; set; }
        public int TotalPrevN { get; set; }
        public int TotalPre_1 { get; set; }
        public int TotaPrevNo { get; set; }
        public int TotalPrevL { get; set; }
        public int TotalPrevY { get; set; }
        public int TotalPrevE { get; set; }
        public string AsmtChange { get; set; }
        public string AsmtChan_1 { get; set; }
        public DateTime SaleDate { get; set; }
        public string Deed { get; set; }
        public float CONVEY_FEE { get; set; }
        public string Owner1 { get; set; }
        public string Owner2 { get; set; }
        public string Owner3 { get; set; }
        public string OwnerBilli { get; set; }
        public string OwnerCityS { get; set; }
        public string OwnerZipCo { get; set; }
        public DateTime OwnerNameC { get; set; }
        public string BuildingTy { get; set; }
        public int Commercial { get; set; }
        public int Residentia { get; set; }
        public string NR_STORIES { get; set; }
        public int YearBuilt { get; set; }
        public int NumberOfFi { get; set; }
        public float PercentAir { get; set; }
        public int NumberOfFu { get; set; }
        public int NumberOfHa { get; set; }
        public int NumberOfRo { get; set; }
        public int NumberOfBe { get; set; }
        public string ATTIC { get; set; }
        public string BASEMENT { get; set; }
        public float BLDG_AREA { get; set; }
        public string ParkingTyp { get; set; }
        public float Calculated { get; set; }
        public float Calculat_1 { get; set; }
        public int ParcelType { get; set; }
        public DateTime ParcelActi { get; set; }

        public Guid CommonParcelId { get; set; }
        public CommonParcelDTO CommonParcel { get; set; }

        public PropertyDTO Property { get; set; }
    }
}
