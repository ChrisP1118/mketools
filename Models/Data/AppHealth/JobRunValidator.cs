using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.AppHealth
{
    public class JobRunValidator : AbstractValidator<JobRun>
    {
        public JobRunValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
