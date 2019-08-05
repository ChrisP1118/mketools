using MkeAlerts.Web.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Middleware.Exceptions
{
    public class IdentityErrorDetails : ErrorDetails
    {
        public List<string> SubErrors { get; set; } = new List<string>();

        public IdentityErrorDetails(IdentityException exception, bool isDev) : base(exception, isDev)
        {
            foreach (var error in exception.Errors)
                SubErrors.Add(error);
        }
    }
}
