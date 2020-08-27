using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Functional
{
    public interface IStreetReferenceService
    {
        Task<List<string>> GetAllStreetDirections(ClaimsPrincipal user, string municipality);
        Task<List<string>> GetAllStreetNames(ClaimsPrincipal user, string municipality);
        Task<List<string>> GetAllStreetTypes(ClaimsPrincipal user, string municipality);
    }
}
