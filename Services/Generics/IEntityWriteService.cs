using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services
{
    public interface IEntityWriteService<TDataModel, TIdType> : IEntityReadService<TDataModel, TIdType>
    {
        Task<TDataModel> Create(ClaimsPrincipal user, TDataModel dataModel);
        Task<IEnumerable<TDataModel>> BulkCreate(ClaimsPrincipal user, IEnumerable<TDataModel> dataModels, bool skipErrors = true);
        //Task<IEnumerable<TDataModel>> Create(ClaimsPrincipal user, IEnumerable<TDataModel> dataModels);
        //Task Detach(ClaimsPrincipal user, TDataModel dataModels);
        //Task Detach(ClaimsPrincipal user, IEnumerable<TDataModel> dataModels);
        Task<TDataModel> Update(ClaimsPrincipal user, TDataModel dataModel);
        Task<TDataModel> Delete(ClaimsPrincipal user, TIdType id);
    }
}
