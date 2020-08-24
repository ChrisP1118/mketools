using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.HistoricPhotos
{
    public class HistoricPhotoLocationValidator : AbstractValidator<HistoricPhotoLocation>
    {
        public HistoricPhotoLocationValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

}
