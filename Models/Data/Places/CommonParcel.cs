using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Places
{
    public class CommonParcel : IHasId<int>, IHasBounds
    {
        public int GetId() => this.MAP_ID;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MAP_ID { get; set; }

        public Geometry Outline { get; set; }

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
