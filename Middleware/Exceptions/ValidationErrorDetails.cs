using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Middleware.Exceptions
{
    public class ValidationErrorDetails : ErrorDetails
    {
        public List<string> SubErrors { get; set; } = new List<string>();

        public ValidationErrorDetails(ValidationException exception, bool isDev) : base (exception, isDev)
        {
            foreach (var error in exception.Errors)
                SubErrors.Add(error.ErrorMessage);
        }
    }
}
