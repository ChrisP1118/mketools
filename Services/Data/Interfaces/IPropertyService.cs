using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Models.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Data.Interfaces
{
    public interface IPropertyService : IEntityWriteService<Property, Guid>
    {
        Task<Dictionary<string, CurrentPropertyRecord>> GetCurrentRecords(ClaimsPrincipal user);
    }
}
