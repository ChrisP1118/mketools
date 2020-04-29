using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Incidents
{
    public class PoliceDispatchCall : IHasId<string>, IHasBounds
    {
        public string GetId() => this.CallNumber;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(12)]
        public string CallNumber { get; set; }

        [Required]
        public DateTime ReportedDateTime { get; set; }

        [Required]
        [MaxLength(60)]
        public string Location { get; set; }

        public int District { get; set; }

        [Required]
        [MaxLength(20)]
        public string NatureOfCall { get; set; }

        [MaxLength(60)]
        public string Status { get; set; }

        //[Column(TypeName = "geometry")]
        public IGeometry Geometry { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MinLat { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MaxLat { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MinLng { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MaxLng { get; set; }

        public GeometryAccuracy Accuracy { get; set; }
        public GeometrySource Source { get; set; }
    }
}
