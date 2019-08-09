using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Incidents
{
    public class DispatchCallValidator : AbstractValidator<DispatchCall>
    {
        public DispatchCallValidator()
        {
            RuleFor(x => x.CallNumber).NotNull().NotEmpty();
        }
    }
}
