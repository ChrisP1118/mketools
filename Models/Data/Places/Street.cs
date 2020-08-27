using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data.Places
{
    public class Street : IHasId<int>, IHasBounds
    {
        public int GetId() => this.CLINEID;

        public Geometry Outline { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MinLat { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MaxLat { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MinLng { get; set; }

        [Column(TypeName = "decimal(13, 10)")]
        public double MaxLng { get; set; }

        public int OBJECTID { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CLINEID { get; set; }

        public int LeftNumberHigh { get; set; }
        public int LeftNumberLow { get; set; }
        public int RightNumberHigh { get; set; }
        public int RightNumberLow { get; set; }

        [MaxLength(2)] public string DIR { get; set; }
        [MaxLength(24)] public string STREET { get; set; }
        [MaxLength(4)] public string STTYPE { get; set; }
        [MaxLength(1)] public string PDIR { get; set; }
        [MaxLength(5)] public string LOW { get; set; }
        [MaxLength(5)] public string HIGH { get; set; }
        [MaxLength(5)] public string LEFTFR { get; set; }
        [MaxLength(5)] public string LEFTTO { get; set; }
        [MaxLength(5)] public string RIGHTFR { get; set; }
        [MaxLength(5)] public string RIGHTTO { get; set; }
        [MaxLength(15)] public string MUNI { get; set; }
        [MaxLength(3)] public string FCC { get; set; }
        [MaxLength(1)] public string OWNER { get; set; }
        [MaxLength(5)] public string R_MCD { get; set; }
        [MaxLength(5)] public string L_MCD { get; set; }
        [MaxLength(1)] public string SHIELD { get; set; }
        [MaxLength(16)] public string HIGHWAY { get; set; }
        [MaxLength(1)] public string SOURCE { get; set; }
        [MaxLength(1)] public string COMMENT { get; set; }
        [MaxLength(1)] public string DATE_CHANG { get; set; }


    }
}
