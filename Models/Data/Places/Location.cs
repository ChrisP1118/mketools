using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Places
{
    public class Location : IHasId<string>
    {
        public string GetId() => this.TAXKEY;

        public IGeometry Outline { get; set; }
        public IPoint Centroid { get; set; }

        public Property Property { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(10)]
        public string TAXKEY { get; set; }
    }
}
