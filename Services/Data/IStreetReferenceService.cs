using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Data
{
    public interface IStreetReferenceService
    {
        Task<List<string>> GetAllStreetDirections(ClaimsPrincipal user);
        Task<List<string>> GetAllStreetNames(ClaimsPrincipal user);
        Task<List<string>> GetAllStreetTypes(ClaimsPrincipal user);
    }
}
