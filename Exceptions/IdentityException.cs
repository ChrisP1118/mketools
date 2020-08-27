using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Exceptions
{
    public class IdentityException : Exception
    {
        public List<string> Errors { get; set; }

        public IdentityException(string message, List<string> errors) : base(message)
        {
            Errors = errors;
        }
    }
}
