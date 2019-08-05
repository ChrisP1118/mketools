using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Exceptions
{
    public class InvalidRequestException : Exception
    {
        public List<string> Errors { get; set; }

        public InvalidRequestException(string message, List<string> errors = null) : base(message)
        {
            Errors = errors;
        }
    }
}
