using MkeAlerts.Web.Models.Data.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Data.Interfaces
{
    public interface ICommonParcelService : IEntityWriteService<CommonParcel, Guid>
    {
        Task RemoveDuplicates();
    }
}
