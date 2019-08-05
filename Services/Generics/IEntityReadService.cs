using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services
{
    public interface IEntityReadService<TDataModel, TIdType>
    {
        Task<List<TDataModel>> GetAll(ClaimsPrincipal user, int offset, int limit, string order, string filter);
        Task<long> GetAllCount(ClaimsPrincipal user, string filter);
        Task<TDataModel> GetOne(ClaimsPrincipal user, TIdType id);
    }
}
