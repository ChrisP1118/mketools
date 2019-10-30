using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Places
{
    public class CommonParcelValidator : AbstractValidator<CommonParcel>
    {
        public CommonParcelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
