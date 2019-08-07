﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services
{
    public interface IEntityWriteService<TDataModel, TIdType> : IEntityReadService<TDataModel, TIdType>
    {
        Task<TDataModel> Create(ClaimsPrincipal user, TDataModel dataModel);
        Task<IEnumerable<TDataModel>> BulkCreate(ClaimsPrincipal user, IList<TDataModel> dataModels, bool skipErrors = true, bool useBulkInsert = true);
        Task<TDataModel> Update(ClaimsPrincipal user, TDataModel dataModel);
        Task<TDataModel> Delete(ClaimsPrincipal user, TIdType id);
    }
}
