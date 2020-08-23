using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.HistoricPhotos
{
    public class HistoricPhotoValidator : AbstractValidator<HistoricPhoto>
    {
        public HistoricPhotoValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
