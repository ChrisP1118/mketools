using GeoAPI.Geometries;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Places
{
    public class LocationDTO
    {
        [MaxLength(10)]
        public string TAXKEY { get; set; }

        [JsonConverter(typeof(GeometryConverter))]
        public IGeometry Outline { get; set; }

        [JsonConverter(typeof(GeometryConverter))]
        public IPoint Centroid { get; set; }

        //[JsonProperty(PropertyName = "Centroid")]
        //public Property SerializableCentroid
        //{
        //    get
        //    {
        //        NetTopologySuite.IO.GeoJsonSerializer.CreateDefault();

        //        var ret = new NTS.Geometries.Point(42.9074, -78.7911);

        //        var jsonSerializer = NTS.IO.GeoJsonSerializer.Create();


        //        var sw = new System.IO.StringWriter();
        //        jsonSerializer.Serialize(sw, ret);

        //        var json = sw.ToString();
        //    }
        //}



    }
}
