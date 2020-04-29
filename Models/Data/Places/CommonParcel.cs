using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Places
{
    public class CommonParcel : IHasId<Guid>, IHasBounds
    {
        public Guid GetId() => this.Id;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public Guid Id { get; set; }

        //[Column(TypeName = "geometry")]
        public IGeometry Outline { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MinLat { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MaxLat { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MinLng { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MaxLng { get; set; }

        public List<Parcel> Parcels { get; set; }

    }
}
