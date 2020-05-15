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

namespace MkeAlerts.Web.Controllers.Functional
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

        protected readonly ILogger<ImportParcelsJob> _importParcelsJobLogger;
        protected readonly ILogger<ImportAddressesJob> _importAddressesJobLogger;
        protected readonly ILogger<ImportStreetsJob> _importStreetsJobLogger;
        protected readonly ILogger<ImportPoliceDispatchCallsJob> _importDispatchCallsJobLogger;
        protected readonly ILogger<ImportFireDispatchCallsJob> _importFireDispatchCallsJobLogger;
        protected readonly ILogger<ImportCrimesJob> _importCrimesJobLogger;

        protected readonly IEntityWriteService<Parcel, string> _parcelWriteService;
        protected readonly IEntityWriteService<Address, int> _addressWriteService;
        protected readonly IEntityWriteService<Street, int> _streetWriteService;
        protected readonly IEntityWriteService<PoliceDispatchCall, string> _dispatchCallWriteService;
        protected readonly IEntityWriteService<FireDispatchCall, string> _fireDispatchCallWriteService;
        protected readonly IEntityWriteService<Crime, string> _crimeWriteService;

        protected readonly IGeocodingService _geocodingService;

        public JobsController(
            IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager,
            IMapper mapper,

            ILogger<ImportParcelsJob> importParcelsJobLogger,
            ILogger<ImportAddressesJob> importAddressesJobLogger,
            ILogger<ImportStreetsJob> importStreetsJobLogger,
            ILogger<ImportPoliceDispatchCallsJob> importDispatchCallsJobLogger,
            ILogger<ImportFireDispatchCallsJob> importFireDispatchCallsJobLogger,
            ILogger<ImportCrimesJob> importCrimesJobLogger,

            IEntityWriteService<Parcel, string> parcelWriteService,
            IEntityWriteService<Address, int> addressWriteService,
            IEntityWriteService<Street, int> streetWriteService,
            IEntityWriteService<PoliceDispatchCall, string> dispatchCallWriteService,
            IEntityWriteService<FireDispatchCall, string> fireDispatchCallWriteService,
            IEntityWriteService<Crime, string> crimeWriteService,

            IGeocodingService geocodingService)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;

            _importParcelsJobLogger = importParcelsJobLogger;
            _importAddressesJobLogger = importAddressesJobLogger;
            _importStreetsJobLogger = importStreetsJobLogger;
            _importDispatchCallsJobLogger = importDispatchCallsJobLogger;
            _importFireDispatchCallsJobLogger = importFireDispatchCallsJobLogger;
            _importCrimesJobLogger = importCrimesJobLogger;

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
            var job1 = BackgroundJob.Enqueue<ImportCommonParcelsJob>(x => x.Run());
            var job2 = BackgroundJob.ContinueJobWith<ImportParcelsJob>(job1, x => x.Run());
            var job3 = BackgroundJob.ContinueJobWith<ImportAddressesJob>(job2, x => x.Run());
            var job4 = BackgroundJob.ContinueJobWith<ImportStreetsJob>(job3, x => x.Run());
            //var job5 = BackgroundJob.ContinueJobWith<ImportCrimesJob>(job4, x => x.Run());
            //var job6 = BackgroundJob.ContinueJobWith<ImportCrimesArchiveJob>(job5, x => x.Run());

            return Ok();
        }

        /// <summary>
        /// Imports common parcels
        /// </summary>
        /// <remarks>
        /// The user making the request must be a site administrator.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("importCommonParcels")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportCommonParcels()
        {
            BackgroundJob.Enqueue<ImportCommonParcelsJob>(x => x.Run());

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
        /// Imports properties archives
        /// </summary>
        /// <remarks>
        /// The user making the request must be a site administrator.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("importPropertiesArchives")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportPropertiesArchives()
        {
            BackgroundJob.Enqueue<ImportPropertiesArchivesJob>(x => x.Run());

            return Ok();
        }

        /// <summary>
        /// Imports police dispatch calls
        /// </summary>
        /// <remarks>
        /// The user making the request must be a site administrator.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("importPoliceDispatchCalls")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportPoliceDispatchCalls()
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

        /// <summary>
        /// Imports crimes archive
        /// </summary>
        /// <remarks>
        /// The user making the request must be a site administrator.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("importCrimesArchive")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportCrimesArchive()
        {
            BackgroundJob.Enqueue<ImportCrimesArchiveJob>(x => x.Run());

            return Ok();
        }

        /// <summary>
        /// Geocodes police dispatch calls
        /// </summary>
        /// <remarks>
        /// The user making the request must be a site administrator.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("geocodePoliceDispatchCalls")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GeocodePoliceDispatchCalls()
        {
            BackgroundJob.Enqueue<GeocodePoliceDispatchCallsJob>(x => x.Run());

            return Ok();
        }

        /// <summary>
        /// Geocodes fire dispatch calls
        /// </summary>
        /// <remarks>
        /// The user making the request must be a site administrator.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("geocodeFireDispatchCalls")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GeocodeFireDispatchCalls()
        {
            BackgroundJob.Enqueue<GeocodeFireDispatchCallsJob>(x => x.Run());

            return Ok();
        }

        /// <summary>
        /// Update next pickup dates notifications
        /// </summary>
        /// <remarks>
        /// The user making the request must be a site administrator.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("updateNextPickupDatesNotifications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateNextPickupDatesNotifications()
        {
            BackgroundJob.Enqueue<UpdateNextPickupDatesNotifications>(x => x.Run());

            return Ok();
        }

        /// <summary>
        /// Send pickup dates notifications
        /// </summary>
        /// <remarks>
        /// The user making the request must be a site administrator.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("sendPickupDatesNotifications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SendPickupDatesNotifications()
        {
            BackgroundJob.Enqueue<SendPickupDatesNotifications>(x => x.Run());

            return Ok();
        }
    }
}
