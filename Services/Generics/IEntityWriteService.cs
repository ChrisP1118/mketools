using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeTools.Web.Services
{
    public interface IEntityWriteService<TDataModel, TIdType> : IEntityReadService<TDataModel, TIdType>
    {
        Task<TDataModel> Create(ClaimsPrincipal user, TDataModel dataModel);
        Task<Tuple<IEnumerable<TDataModel>, IEnumerable<TDataModel>>> BulkUpsert(ClaimsPrincipal user, IList<TDataModel> dataModels, bool useBulkInsert = true);
        Task<TDataModel> Update(ClaimsPrincipal user, TDataModel dataModel);
        Task<IEnumerable<TDataModel>> BulkUpdate(ClaimsPrincipal user, IEnumerable<TDataModel> dataModels);
        Task<TDataModel> Delete(ClaimsPrincipal user, TIdType id);
    }
}
