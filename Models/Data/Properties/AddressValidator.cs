using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Properties
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.FormattedAddress).NotNull().NotEmpty();
            RuleFor(x => x.TAXKEY).NotNull().NotEmpty();
        }
    }
}
