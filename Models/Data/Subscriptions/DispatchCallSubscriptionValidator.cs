using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data.Subscriptions
{
    public class DispatchCallSubscriptionValidator : AbstractValidator<DispatchCallSubscription>
    {
        public DispatchCallSubscriptionValidator()
        {
            RuleFor(x => x.ApplicationUserId).NotNull().NotEmpty();
        }
    }
}
