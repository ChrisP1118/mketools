using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data.Subscriptions
{
    public class PickupDatesSubscriptionValidator : AbstractValidator<PickupDatesSubscription>
    {
        public PickupDatesSubscriptionValidator()
        {
            RuleFor(x => x.ApplicationUserId).NotNull().NotEmpty();
        }
    }
}
