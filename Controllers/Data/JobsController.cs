using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Jobs;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.DispatchCalls;
using MkeAlerts.Web.Models.Data.Properties;
using MkeAlerts.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Controllers.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        protected readonly IConfiguration _configuration;
        protected readonly IMapper _mapper;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        protected readonly UserManager<ApplicationUser> _userManager;

        protected readonly ILogger<ImportDispatchCallsJob> _importDispatchCallsJobLogger;

        protected readonly IEntityWriteService<Property, string> _propertyWriteService;
        protected readonly IEntityWriteService<Address, string> _addressWriteService;
        protected readonly IEntityWriteService<DispatchCall, string> _dispatchCallWriteService;

        public JobsController(
            IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager,
            IMapper mapper,

            ILogger<ImportDispatchCallsJob> importDispatchCallsJobLogger,

            IEntityWriteService<Property, string> propertyWriteService,
            IEntityWriteService<Address, string> addressWriteService,
            IEntityWriteService<DispatchCall, string> dispatchCallWriteService)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;

            _importDispatchCallsJobLogger = importDispatchCallsJobLogger;

            _propertyWriteService = propertyWriteService;
            _addressWriteService = addressWriteService;
            _dispatchCallWriteService = dispatchCallWriteService;
        }

        /// <summary>
        /// Imports properties
        /// </summary>
        /// <returns></returns>
        [HttpPost("ImportProperties")]
        [ActionName("ImportProperties")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportProperties()
        {
            //BackgroundJob.Enqueue<ImportPropertiesJob>(x => x.Run(true));

            ImportPropertiesJob job = new ImportPropertiesJob(_configuration, _signInManager, _userManager, _propertyWriteService);
            string results = await job.Run();

            return Ok("Started");
        }

        /// <summary>
        /// Imports properties
        /// </summary>
        /// <returns></returns>
        [HttpPost("ImportAddresses")]
        [ActionName("ImportAddresses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportAddresses()
        {
            ImportAddressesJob job = new ImportAddressesJob(_configuration, _signInManager, _userManager, _addressWriteService);
            string results = await job.Run();

            return Ok(results);
        }

        /// <summary>
        /// Imports dispatch calls
        /// </summary>
        /// <returns></returns>
        [HttpPost("ImportDispatchCalls")]
        [ActionName("ImportDispatchCalls")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportDispatchCalls()
        {
            ImportDispatchCallsJob job = new ImportDispatchCallsJob(_configuration, _signInManager, _userManager, _importDispatchCallsJobLogger, _dispatchCallWriteService);
            await job.Run();

            return Ok();
        }
    }
}
