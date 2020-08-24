using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.HistoricPhotos
{
    public class HistoricPhoto : IHasId<string>
    {
        public string GetId() => this.Id;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(20)]
        public string Id { get; set; }

        [MaxLength(12)]
        public string Collection { get; set; }

        public Guid? HistoricPhotoLocationId { get; set; }
        public HistoricPhotoLocation HistoricPhotoLocation { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        [MaxLength(50)]
        public string Date { get; set; }

        public int? Year { get; set; }

        [MaxLength(250)]
        public string Place { get; set; }

        [MaxLength(250)]
        public string CurrentAddress { get; set; }

        [MaxLength(250)]
        public string OldAddress { get; set; }

        [MaxLength(250)]
        public string ImageUrl { get; set; }

        [MaxLength(250)]
        public string Url { get; set; }
    }
}
