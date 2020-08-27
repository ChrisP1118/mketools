using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeTools.Web.Services
{
    public interface IRelationshipReadService<TDataModel>
    {
        Task<List<TDataModel>> GetAll(ClaimsPrincipal user, Guid parentId, int offset, int limit, string order, string filter);
        Task<long> GetAllCount(ClaimsPrincipal user, Guid parentId, string filter);
        Task<TDataModel> GetOne(ClaimsPrincipal user, Guid parentId, Guid id);

        Guid GetParentId(TDataModel dataModel);
        Guid GetChildId(TDataModel dataModel);
    }
}
