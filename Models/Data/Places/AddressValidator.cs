using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data.Places
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.ADDRESS_ID).NotNull().NotEmpty().NotEqual(0);
            RuleFor(x => x.TAXKEY).NotNull().NotEmpty();
        }
    }
}
