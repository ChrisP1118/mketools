using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
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
using MkeAlerts.Web.Services.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Controllers.Data
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = ApplicationRole.SiteAdminRole)]
    public class JobsController : ControllerBase
    {
        protected readonly IConfiguration _configuration;
        protected readonly IMapper _mapper;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        protected readonly UserManager<ApplicationUser> _userManager;

        protected readonly ILogger<ImportPropertiesJob> _importPropertiesJobLogger;
        protected readonly ILogger<ImportParcelsJob> _importParcelsJobLogger;
        protected readonly ILogger<ImportAddressesJob> _importAddressesJobLogger;
        protected readonly ILogger<ImportStreetsJob> _importStreetsJobLogger;
        protected readonly ILogger<ImportPoliceDispatchCallsJob> _importDispatchCallsJobLogger;
        protected readonly ILogger<ImportFireDispatchCallsJob> _importFireDispatchCallsJobLogger;
        protected readonly ILogger<ImportCrimesJob> _importCrimesJobLogger;

        protected readonly IEntityWriteService<Property, string> _propertyWriteService;
        protected readonly IEntityWriteService<Parcel, string> _parcelWriteService;
        protected readonly IEntityWriteService<Address, string> _addressWriteService;
        protected readonly IEntityWriteService<Street, string> _streetWriteService;
        protected readonly IEntityWriteService<PoliceDispatchCall, string> _dispatchCallWriteService;
        protected readonly IEntityWriteService<FireDispatchCall, string> _fireDispatchCallWriteService;
        protected readonly IEntityWriteService<Crime, string> _crimeWriteService;

        protected readonly IGeocodingService _geocodingService;

        public JobsController(
            IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager,
            IMapper mapper,

            ILogger<ImportPropertiesJob> importPropertiesJobLogger,
            ILogger<ImportParcelsJob> importParcelsJobLogger,
            ILogger<ImportAddressesJob> importAddressesJobLogger,
            ILogger<ImportStreetsJob> importStreetsJobLogger,
            ILogger<ImportPoliceDispatchCallsJob> importDispatchCallsJobLogger,
            ILogger<ImportFireDispatchCallsJob> importFireDispatchCallsJobLogger,
            ILogger<ImportCrimesJob> importCrimesJobLogger,

            IEntityWriteService<Property, string> propertyWriteService,
            IEntityWriteService<Parcel, string> parcelWriteService,
            IEntityWriteService<Address, string> addressWriteService,
            IEntityWriteService<Street, string> streetWriteService,
            IEntityWriteService<PoliceDispatchCall, string> dispatchCallWriteService,
            IEntityWriteService<FireDispatchCall, string> fireDispatchCallWriteService,
            IEntityWriteService<Crime, string> crimeWriteService,

            IGeocodingService geocodingService)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;

            _importPropertiesJobLogger = importPropertiesJobLogger;
            _importParcelsJobLogger = importParcelsJobLogger;
            _importAddressesJobLogger = importAddressesJobLogger;
            _importStreetsJobLogger = importStreetsJobLogger;
            _importDispatchCallsJobLogger = importDispatchCallsJobLogger;
            _importFireDispatchCallsJobLogger = importFireDispatchCallsJobLogger;
            _importCrimesJobLogger = importCrimesJobLogger;

            _propertyWriteService = propertyWriteService;
            _parcelWriteService = parcelWriteService;
            _addressWriteService = addressWriteService;
            _streetWriteService = streetWriteService;
            _dispatchCallWriteService = dispatchCallWriteService;
            _fireDispatchCallWriteService = fireDispatchCallWriteService;
            _crimeWriteService = crimeWriteService;

            _geocodingService = geocodingService;
        }

        /// <summary>
        /// Imports all baseline data
        /// </summary>
        /// <remarks>
        /// The user making the request must be a site administrator.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("importBaselines")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportBaselines()
        {
            var job1 = BackgroundJob.Enqueue<ImportPropertiesJob>(x => x.Run());
            var job2 = BackgroundJob.ContinueJobWith<ImportParcelsJob>(job1, x => x.Run());
            var job3 = BackgroundJob.ContinueJobWith<ImportAddressesJob>(job2, x => x.Run());
            var job4 = BackgroundJob.ContinueJobWith<ImportStreetsJob>(job3, x => x.Run());
            var job5 = BackgroundJob.ContinueJobWith<ImportCrimesJob>(job4, x => x.Run());


            //ImportPropertiesJob importPropertiesJob = new ImportPropertiesJob(_configuration, _signInManager, _userManager, _importPropertiesJobLogger, _propertyWriteService);
            //ImportParcelsJob importParcelsJob = new ImportParcelsJob(_configuration, _signInManager, _userManager, _importParcelsJobLogger, _parcelWriteService);
            //ImportAddressesJob importAddressesJob = new ImportAddressesJob(_configuration, _signInManager, _userManager, _importAddressesJobLogger, _addressWriteService);
            //ImportStreetsJob importStreetsJob = new ImportStreetsJob(_configuration, _signInManager, _userManager, _importStreetsJobLogger, _streetWriteService);
            //ImportCrimesJob importCrimesJob = new ImportCrimesJob(_configuration, _signInManager, _userManager, _importCrimesJobLogger, _crimeWriteService);

            //await importPropertiesJob.Run();

            //List<Task> tasks = new List<Task>();
            //tasks.Add(importParcelsJob.Run());
            //tasks.Add(importAddressesJob.Run());
            //tasks.Add(importStreetsJob.Run());
            //tasks.Add(importCrimesJob.Run());

            //await Task.WhenAll(tasks);

            return Ok();
        }

        /// <summary>
        /// Imports properties
        /// </summary>
        /// <remarks>
        /// The user making the request must be a site administrator.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("importProperties")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportProperties()
        {
            BackgroundJob.Enqueue<ImportPropertiesJob>(x => x.Run());

            return Ok();
        }

        /// <summary>
        /// Imports parcels
        /// </summary>
        /// <remarks>
        /// The user making the request must be a site administrator.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("importParcels")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportParcels()
        {
            BackgroundJob.Enqueue<ImportParcelsJob>(x => x.Run());

            return Ok();
        }

        /// <summary>
        /// Imports address
        /// </summary>
        /// <remarks>
        /// The user making the request must be a site administrator.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("importAddresses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportAddresses()
        {
            BackgroundJob.Enqueue<ImportAddressesJob>(x => x.Run());

            return Ok();
        }

        /// <summary>
        /// Imports streets
        /// </summary>
        /// <remarks>
        /// The user making the request must be a site administrator.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("importStreets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportStreets()
        {
            BackgroundJob.Enqueue<ImportStreetsJob>(x => x.Run());

            return Ok();
        }

        /// <summary>
        /// Imports dispatch calls
        /// </summary>
        /// <remarks>
        /// The user making the request must be a site administrator.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("importDispatchCalls")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportDispatchCalls()
        {
            BackgroundJob.Enqueue<ImportPoliceDispatchCallsJob>(x => x.Run());

            return Ok();
        }

        /// <summary>
        /// Imports fire dispatch calls
        /// </summary>
        /// <remarks>
        /// The user making the request must be a site administrator.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("importFireDispatchCalls")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportFireDispatchCalls()
        {
            BackgroundJob.Enqueue<ImportFireDispatchCallsJob>(x => x.Run());

            return Ok();
        }

        /// <summary>
        /// Imports crimes
        /// </summary>
        /// <remarks>
        /// The user making the request must be a site administrator.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("importCrimes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportCrimes()
        {
            BackgroundJob.Enqueue<ImportCrimesJob>(x => x.Run());

            return Ok();
        }

    }
}
