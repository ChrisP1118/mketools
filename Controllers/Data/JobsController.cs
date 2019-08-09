using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Jobs;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Incidents;
using MkeAlerts.Web.Models.Data.Places;
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

        protected readonly ILogger<ImportPropertiesJob> _importPropertiesJobLogger;
        protected readonly ILogger<ImportLocationsJob> _importLocationsJobLogger;
        protected readonly ILogger<ImportAddressesJob> _importAddressesJobLogger;
        protected readonly ILogger<ImportStreetsJob> _importStreetsJobLogger;
        protected readonly ILogger<ImportDispatchCallsJob> _importDispatchCallsJobLogger;
        protected readonly ILogger<ImportCrimesJob> _importCrimesJobLogger;

        protected readonly IEntityWriteService<Property, string> _propertyWriteService;
        protected readonly IEntityWriteService<Location, string> _locationWriteService;
        protected readonly IEntityWriteService<Address, string> _addressWriteService;
        protected readonly IEntityWriteService<Street, string> _streetWriteService;
        protected readonly IEntityWriteService<DispatchCall, string> _dispatchCallWriteService;
        protected readonly IEntityWriteService<Crime, string> _crimeWriteService;

        protected readonly IGeocodingService _geocodingService;

        public JobsController(
            IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager,
            IMapper mapper,

            ILogger<ImportPropertiesJob> importPropertiesJobLogger,
            ILogger<ImportLocationsJob> importLocationsJobLogger,
            ILogger<ImportAddressesJob> importAddressesJobLogger,
            ILogger<ImportStreetsJob> importStreetsJobLogger,
            ILogger<ImportDispatchCallsJob> importDispatchCallsJobLogger,
            ILogger<ImportCrimesJob> importCrimesJobLogger,

            IEntityWriteService<Property, string> propertyWriteService,
            IEntityWriteService<Location, string> locationWriteService,
            IEntityWriteService<Address, string> addressWriteService,
            IEntityWriteService<Street, string> streetWriteService,
            IEntityWriteService<DispatchCall, string> dispatchCallWriteService,
            IEntityWriteService<Crime, string> crimeWriteService,

            IGeocodingService geocodingService)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;

            _importPropertiesJobLogger = importPropertiesJobLogger;
            _importLocationsJobLogger = importLocationsJobLogger;
            _importAddressesJobLogger = importAddressesJobLogger;
            _importStreetsJobLogger = importStreetsJobLogger;
            _importDispatchCallsJobLogger = importDispatchCallsJobLogger;
            _importCrimesJobLogger = importCrimesJobLogger;

            _propertyWriteService = propertyWriteService;
            _locationWriteService = locationWriteService;
            _addressWriteService = addressWriteService;
            _streetWriteService = streetWriteService;
            _dispatchCallWriteService = dispatchCallWriteService;
            _crimeWriteService = crimeWriteService;

            _geocodingService = geocodingService;
        }

        /// <summary>
        /// Imports all baseline data
        /// </summary>
        /// <returns></returns>
        [HttpPost("ImportBaselines")]
        [ActionName("ImportBaselines")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportBaselines()
        {
            ImportPropertiesJob importPropertiesJob = new ImportPropertiesJob(_configuration, _signInManager, _userManager, _importPropertiesJobLogger, _propertyWriteService);
            ImportLocationsJob importLocationsJob = new ImportLocationsJob(_configuration, _signInManager, _userManager, _importLocationsJobLogger, _locationWriteService);
            ImportAddressesJob importAddressesJob = new ImportAddressesJob(_configuration, _signInManager, _userManager, _importAddressesJobLogger, _addressWriteService);
            ImportStreetsJob importStreetsJob = new ImportStreetsJob(_configuration, _signInManager, _userManager, _importStreetsJobLogger, _streetWriteService);
            ImportCrimesJob importCrimesJob = new ImportCrimesJob(_configuration, _signInManager, _userManager, _importCrimesJobLogger, _crimeWriteService);

            await importPropertiesJob.Run();

            List<Task> tasks = new List<Task>();
            tasks.Add(importLocationsJob.Run());
            tasks.Add(importAddressesJob.Run());
            tasks.Add(importStreetsJob.Run());
            tasks.Add(importCrimesJob.Run());

            await Task.WhenAll(tasks);

            return Ok();
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

            ImportPropertiesJob job = new ImportPropertiesJob(_configuration, _signInManager, _userManager, _importPropertiesJobLogger, _propertyWriteService);
            await job.Run();

            return Ok();
        }

        /// <summary>
        /// Imports locations
        /// </summary>
        /// <returns></returns>
        [HttpPost("ImportLocations")]
        [ActionName("ImportLocations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportLocations()
        {
            ImportLocationsJob job = new ImportLocationsJob(_configuration, _signInManager, _userManager, _importLocationsJobLogger, _locationWriteService);
            await job.Run();

            return Ok();
        }

        /// <summary>
        /// Imports address
        /// </summary>
        /// <returns></returns>
        [HttpPost("ImportAddresses")]
        [ActionName("ImportAddresses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportAddresses()
        {
            ImportAddressesJob job = new ImportAddressesJob(_configuration, _signInManager, _userManager, _importAddressesJobLogger, _addressWriteService);
            await job.Run();

            return Ok();
        }

        /// <summary>
        /// Imports streets
        /// </summary>
        /// <returns></returns>
        [HttpPost("ImportStreets")]
        [ActionName("ImportStreets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportStreets()
        {
            ImportStreetsJob job = new ImportStreetsJob(_configuration, _signInManager, _userManager, _importStreetsJobLogger, _streetWriteService);
            await job.Run();

            return Ok();
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
            ImportDispatchCallsJob job = new ImportDispatchCallsJob(_configuration, _signInManager, _userManager, _importDispatchCallsJobLogger, _dispatchCallWriteService, _geocodingService);
            await job.Run();

            return Ok();
        }

        /// <summary>
        /// Imports crimes
        /// </summary>
        /// <returns></returns>
        [HttpPost("ImportCrimes")]
        [ActionName("ImportCrimes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportCrimes()
        {
            ImportCrimesJob job = new ImportCrimesJob(_configuration, _signInManager, _userManager, _importCrimesJobLogger, _crimeWriteService);
            await job.Run();

            return Ok();
        }

    }
}
