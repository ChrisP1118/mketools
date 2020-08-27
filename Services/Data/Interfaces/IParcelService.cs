using MkeTools.Web.Models.Data.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Data.Interfaces
{
    public interface IParcelService : IEntityWriteService<Parcel, string>
    {
        Task<HashSet<string>> GetAllTaxkeys(ClaimsPrincipal user);
    }
}
