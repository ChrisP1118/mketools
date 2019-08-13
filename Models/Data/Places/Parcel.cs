using GeoAPI.Geometries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Places
{
    public class Parcel : IHasId<string>, IHasBounds
    {
        public string GetId() => this.TAXKEY;

        //[Column(TypeName = "geometry")]
        public IGeometry Outline { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public double MinLat { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public double MaxLat { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public double MinLng { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public double MaxLng { get; set; }

        public Property Property { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(10)]
        public string TAXKEY { get; set; }


    }
}
