using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data.Incidents
{
    public class PoliceDispatchCall : IHasId<string>, IHasBounds, IGeocodable
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
        public Geometry Geometry { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MinLat { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MaxLat { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MinLng { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MaxLng { get; set; }

        public GeometryAccuracy? Accuracy { get; set; }
        public GeometrySource? Source { get; set; }

        public DateTime? LastGeocodeAttempt { get; set; }
    }
}
