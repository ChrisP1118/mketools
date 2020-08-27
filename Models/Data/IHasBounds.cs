using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data
{
    public interface IHasBounds
    {
        double MinLat { get; set; }

        double MaxLat { get; set; }

        double MinLng { get; set; }

        double MaxLng { get; set; }
    }
}
