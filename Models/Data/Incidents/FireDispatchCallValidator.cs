using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data.Incidents
{
    public class FireDispatchCallValidator : AbstractValidator<FireDispatchCall>
    {
        public FireDispatchCallValidator()
        {
            RuleFor(x => x.CFS).NotNull().NotEmpty();
        }
    }
}
