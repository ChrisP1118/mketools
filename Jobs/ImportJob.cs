using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Models.Data.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Jobs
{
    public abstract class ImportJob
    {
        protected readonly IConfiguration _configuration;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        protected readonly UserManager<ApplicationUser> _userManager;

        public ImportJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        protected async Task<ClaimsPrincipal> GetClaimsPrincipal()
        {
            ApplicationUser applicationUser = await _userManager.FindByIdAsync(_configuration["JobUserId"]);
            ClaimsPrincipal claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(applicationUser);

            return claimsPrincipal;
        }
    }
}
