using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data
{
    public interface IHasId<TIdType>
    {
        TIdType GetId();
    }
}
