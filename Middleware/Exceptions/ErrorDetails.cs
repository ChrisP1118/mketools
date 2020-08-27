using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Middleware.Exceptions
{
    public class ErrorDetails
    {
        public string Message { get; set; }
        public string Details { get; set; }

        public string StackTrace { get; set; }

        public ErrorDetails(Exception exception, bool isDev)
        {
            Message = exception.GetType().Name;
            Details = exception.Message;

            if (isDev)
            {
                StackTrace = exception.StackTrace;
            }
        }
    }
}
