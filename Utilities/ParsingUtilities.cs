using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Utilities
{
    public class ParsingUtilities
    {
        public static int ParseInt(string value, int defaultValue = 0)
        {
            int n = defaultValue;
            int.TryParse(value, out n);
            return n;
        }

    }
}
