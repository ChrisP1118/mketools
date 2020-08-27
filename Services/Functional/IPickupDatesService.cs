using MkeTools.Web.Models.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Functional
{
    public interface IPickupDatesService
    {
        Task<PickupDatesResults> GetPickupDates(string laddr, string sdir, string sname, string stype);
    }
}
