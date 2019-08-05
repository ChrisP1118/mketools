using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services
{
    public interface IRelationshipWriteService<TDataModel> : IRelationshipReadService<TDataModel>
    {
        Task<TDataModel> Create(ClaimsPrincipal user, Guid parentId, TDataModel dataModel);
        Task<TDataModel> Update(ClaimsPrincipal user, Guid parentId, TDataModel dataModel);
        Task<TDataModel> Delete(ClaimsPrincipal user, Guid parentId, Guid id);
    }
}
