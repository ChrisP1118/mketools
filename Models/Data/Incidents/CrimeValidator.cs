using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data.Incidents
{
    public class CrimeValidator : AbstractValidator<Crime>
    {
        public CrimeValidator()
        {
            RuleFor(x => x.IncidentNum).NotNull().NotEmpty();
        }
    }
}
