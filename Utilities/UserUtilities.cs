using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeTools.Web.Utilities
{
    public static class UserUtilities
    {
        public static ClaimsPrincipal GetClaimsPrincipal(Guid applicationUserId)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, applicationUserId.ToString())
            }));
        }
    }
}
