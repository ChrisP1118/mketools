using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Incidents
{
    public class FireDispatchCall : IHasId<string>, IHasBounds
    {
        public string GetId() => this.CFS;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(12)]
        public string CFS { get; set; }

        [Required]
        public DateTime ReportedDateTime { get; set; }

        [Required]
        [MaxLength(60)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string Apt { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [MaxLength(40)]
        public string NatureOfCall { get; set; }

        [MaxLength(60)]
        public string Disposition { get; set; }

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

        public GeometryAccuracy Accuracy { get; set; }
        public GeometrySource Source { get; set; }
    }
}
