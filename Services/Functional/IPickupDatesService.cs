using MkeAlerts.Web.Models.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Functional
{
    public interface IPickupDatesService
    {
        Task<PickupDatesResults> GetPickupDates(string number, string direction, string street, string suffix);
    }
}
