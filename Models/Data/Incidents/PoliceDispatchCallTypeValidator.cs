using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data.Incidents
{
    public class PoliceDispatchCallTypeValidator : AbstractValidator<PoliceDispatchCallType>
    {
        public PoliceDispatchCallTypeValidator()
        {
            RuleFor(x => x.NatureOfCall).NotNull().NotEmpty();
        }
    }
}
