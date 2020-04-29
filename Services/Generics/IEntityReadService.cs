using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services
{
    public interface IEntityReadService<TDataModel, TIdType>
    {
        Task<List<TDataModel>> GetAll(ClaimsPrincipal user, int offset, int limit, string order, string includes, string filter, double? northBound, double? southBound, double? eastBound, double? westBound, bool useHighPrecision, bool noTracking, Func<IQueryable<TDataModel>, IQueryable<TDataModel>> filterFunc);
        Task<long> GetAllCount(ClaimsPrincipal user, string filter, double? northBound, double? southBound, double? eastBound, double? westBound, bool useHighPrecision);
        Task<TDataModel> GetOne(ClaimsPrincipal user, TIdType id, string includes);
    }
}
