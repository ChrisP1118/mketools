using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Properties
{
    public class StreetValidator : AbstractValidator<Street>
    {
        public StreetValidator()
        {
            RuleFor(x => x.NEWDIME_ID).NotNull().NotEmpty();
        }
    }
}
