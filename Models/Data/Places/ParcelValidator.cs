using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Places
{
    public class ParcelValidator : AbstractValidator<Parcel>
    {
        public ParcelValidator()
        {
            RuleFor(x => x.TAXKEY).NotNull().NotEmpty();
            RuleFor(x => x.MAP_ID).NotNull().NotEmpty().NotEqual(0);
        }
    }
}
