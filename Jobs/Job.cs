using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MkeTools.Web.Models.Data.Accounts;
using MkeTools.Web.Models.Data.AppHealth;
using MkeTools.Web.Services.Data.Interfaces;
using MkeTools.Web.Services.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeTools.Web.Jobs
{
    public abstract class Job
    {
        protected readonly IConfiguration _configuration;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly IMailerService _mailerService;

        public Job(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _mailerService = mailerService;
        }

        protected async Task<ClaimsPrincipal> GetClaimsPrincipal()
        {
            ApplicationUser applicationUser = await _userManager.FindByIdAsync(_configuration["JobUserId"]);
            ClaimsPrincipal claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(applicationUser);

            return claimsPrincipal;
        }
    }
}
