using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Incidents
{
    public class FireDispatchCallTypeValidator : AbstractValidator<FireDispatchCallType>
    {
        public FireDispatchCallTypeValidator()
        {
            RuleFor(x => x.NatureOfCall).NotNull().NotEmpty();
        }
    }
}
