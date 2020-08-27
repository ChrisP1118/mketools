using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MkeTools.Web.Utilities
{
    public class ParsingUtilities
    {
        public static int ParseInt(string value, int defaultValue = 0, bool stripNonNumberic = false)
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            int n = defaultValue;
            if (stripNonNumberic)
                value = Regex.Replace(value, "[^0-9.]", "");
            int.TryParse(value, out n);
            return n;
        }
    }
}
