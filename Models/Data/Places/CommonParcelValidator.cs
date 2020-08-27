using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data.Places
{
    public class CommonParcelValidator : AbstractValidator<CommonParcel>
    {
        public CommonParcelValidator()
        {
            RuleFor(x => x.MAP_ID).NotNull().NotEmpty().NotEqual(0);
        }
    }
}
