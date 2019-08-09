using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Places
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            //RuleFor(x => x.FormattedAddress).NotNull().NotEmpty();
            RuleFor(x => x.RCD_NBR).NotNull().NotEmpty();
            RuleFor(x => x.TAXKEY).NotNull().NotEmpty();
        }
    }
}
