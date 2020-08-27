using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data.HistoricPhotos
{
    public class HistoricPhotoLocation : IHasId<Guid>, IHasBounds, IGeocodable
    {
        public Guid GetId() => this.Id;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public Guid Id { get; set; }

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

        public List<HistoricPhoto> HistoricPhotos { get; set; }
    }
}
