using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data.Places
{
    public class StreetValidator : AbstractValidator<Street>
    {
        public StreetValidator()
        {
            RuleFor(x => x.CLINEID).NotNull().NotEmpty().NotEqual(0);
        }
    }
}
