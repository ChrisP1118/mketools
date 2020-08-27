using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data
{
    public interface IHasId<TIdType>
    {
        TIdType GetId();
    }
}
