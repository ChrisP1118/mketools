using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Places
{
    public class MpropField
    {
        public string DataModelName { get; set; }
        public List<string> DataFileNames { get; set; } = new List<string>();
        public List<int> Years { get; set; } = new List<int>();
        public PropertyInfo PropertyInfo { get; set; }
    }
}
