using MkeTools.Web.Models.DTO.Accounts;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MkeTools.Web.Models.Data.Accounts;
using Microsoft.AspNetCore.Http;
using MkeTools.Web.Exceptions;
using MkeTools.Web.Middleware.Exceptions;
using Hangfire;
using MkeTools.Web.Data;
using Microsoft.EntityFrameworkCore;
using MkeTools.Web.Utilities;
using MkeTools.Web.Services.Functional;
using System.Net;

namespace MkeTools.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IMailerService _mailerService;

        public AccountController(
            ApplicationDbContext dbContext,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IMapper mapper,
            IMailerService mailerService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
            _mailerService = mailerService;
        }

        /// <summary>
        /// Authenticates a user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns data about the user account and a JWT token</returns>
        /// <response code="200">Successful authentication</response>
        /// <response code="400">Invalid username or password</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(LoginResultsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IdentityErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<LoginResultsDTO> Login([FromBody] LoginDTO model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                var applicationUser = _userManager.Users.SingleOrDefault(r => r.UserName == model.Email);
                var roles = await _userManager.GetRolesAsync(applicationUser);

                return new LoginResultsDTO()
                {
                    UserName = applicationUser.UserName,
                    Id = applicationUser.Id,
                    Roles = roles.ToList(),
                    JwtToken = await GenerateJwtToken(applicationUser)
                };
            }

            ExternalCredential externalCredential = await _dbContext.ExternalCredentials
                .Where(ec => ec.ApplicationUser.Email == model.Email)
                .FirstOrDefaultAsync();

            List<string> subErrors = new List<string>();

            if (externalCredential != null)
                subErrors.Add("That email address is used by a " + externalCredential.Provider + " account. Try signing in with " + externalCredential.Provider + ".");

            throw new IdentityException("Invalid username or password", subErrors);
        }

        /// <summary>
        /// Authenticates a user using external credentials
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns data about the user account and a JWT token</returns>
        /// <response code="200">Successful authentication</response>
        /// <response code="400">Invalid username or password</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(LoginResultsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IdentityErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<LoginResultsDTO> LoginExternalCredential([FromBody] LoginExternalCredentialDTO model)
        {
            ExternalCredential externalCredential = await _dbContext.ExternalCredentials
                .Include(ec => ec.ApplicationUser)
                .Where(ec => ec.Provider == model.Provider && ec.ExternalId == model.ExternalId)
                .FirstOrDefaultAsync();

            ApplicationUser applicationUser = null;

            if (externalCredential == null)
            {
                ApplicationUser conflictingUser = await _dbContext.ApplicationUsers
                    .Include(au => au.ExternalCredentials)
                    .Where(au => au.Email == model.Email)
                    .FirstOrDefaultAsync();

                if (conflictingUser != null)
                {
                    if (conflictingUser.ExternalCredentials.Count > 0)
                        throw new IdentityException("That email address is already in use.", new List<string>()
                        {
                            "Try logging in with " + conflictingUser.ExternalCredentials.First().Provider + "."
                        });
                    else
                        throw new IdentityException("That email address is already in use.", new List<string>()
                        {
                            "Try logging in with a username and password."
                        });
                }

                applicationUser = new ApplicationUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true,
                    ExternalCredentials = new List<ExternalCredential>()
                    {
                        new ExternalCredential
                        {
                            Id = Guid.NewGuid(),
                            Provider = model.Provider,
                            ExternalId = model.ExternalId
                        }
                    }
                };

                IdentityResult result = await _userManager.CreateAsync(applicationUser);
                if (!result.Succeeded)
                    throw new IdentityException("Error with external auth", new List<string>());
            }
            else
            {
                applicationUser = externalCredential.ApplicationUser;
                await _signInManager.SignInAsync(applicationUser, false);
            }

            var roles = await _userManager.GetRolesAsync(applicationUser);

            return new LoginResultsDTO()
            {
                UserName = applicationUser.UserName,
                Id = applicationUser.Id,
                Roles = roles.ToList(),
                JwtToken = await GenerateJwtToken(applicationUser)
            };
        }

        /// <summary>
        /// Registers an account
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns data about the user account and a JWT token</returns>
        /// <response code="200">Account was successfully created</response>
        /// <response code="400">Account could not be created</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(RegisterResultsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IdentityErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<RegisterResultsDTO> Register([FromBody] RegisterDTO model)
        {
            var applicationUser = _mapper.Map<RegisterDTO, ApplicationUser>(model);
            applicationUser.UserName = model.Email;

            var result = await _userManager.CreateAsync(applicationUser, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(applicationUser, false);

                var roles = await _userManager.GetRolesAsync(applicationUser);

                return new RegisterResultsDTO()
                {
                    UserName = applicationUser.UserName,
                    Id = applicationUser.Id,
                    Roles = roles.ToList(),
                    JwtToken = await GenerateJwtToken(applicationUser)
                };
            }

            throw new IdentityException("Unable to create account", result.Errors.Select(e => e.Description).ToList());
        }

        /// <summary>
        /// Requests a password reset email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(RequestPasswordResetResultsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IdentityErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<RequestPasswordResetResultsDTO> RequestPasswordReset([FromBody] RequestPasswordResetDTO model)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                throw new IdentityException("Invalid email address", new List<string>());

            // TODO: Make sure user has confirmed email
            //if (!await _userManager.IsEmailConfirmedAsync(user))
            //    throw new IdentityException("Email address has not been confirmed", new List<string>());

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string resetPasswordUrl = _configuration["ResetPasswordUrl"];

            resetPasswordUrl += (resetPasswordUrl.Contains("?") ? "&" : "?") + "token=" + WebUtility.UrlEncode(token);

            await _mailerService.SendEmail(
                user.Email,
                "Password Reset Request",
                "To reset your password for MKE Tools, click on the following link or visit this URL: " + resetPasswordUrl
            );

            return new RequestPasswordResetResultsDTO();
        }

        /// <summary>
        /// Resets a password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(ResetPasswordResultsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IdentityErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<ResetPasswordResultsDTO> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                throw new IdentityException("Invalid email address", new List<string>());

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);

                var roles = await _userManager.GetRolesAsync(user);

                return new ResetPasswordResultsDTO()
                {
                    UserName = user.UserName,
                    Id = user.Id,
                    Roles = roles.ToList(),
                    JwtToken = await GenerateJwtToken(user)
                };
            }

            throw new IdentityException("Unable to reset password", result.Errors.Select(e => e.Description).ToList());
        }

        /// <summary>
        /// Tests to see if a user is authenticated
        /// </summary>
        /// <remarks>This can be used to determine if an authentication token is valid</remarks>
        /// <returns></returns>
        /// <response code="200">User is authenticated</response>
        /// <response code="401">User is not authenticated</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }

        private async Task<string> GenerateJwtToken(ApplicationUser applicationUser)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, applicationUser.Id.ToString())
            };

            var roles = await _userManager.GetRolesAsync(applicationUser);
            foreach (string role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
