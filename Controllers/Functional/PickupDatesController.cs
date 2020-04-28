using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Filters.Support;
using MkeAlerts.Web.Middleware.Exceptions;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Models.DTO.Geocoding;
using MkeAlerts.Web.Models.DTO.PickupDates;
using MkeAlerts.Web.Models.Internal;
using MkeAlerts.Web.Services;
using MkeAlerts.Web.Services.Data;
using MkeAlerts.Web.Services.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Controllers.Functional
{
    [ApiController]
    [Route("api/[controller]")]
    public class PickupDatesController : ControllerBase
    {
        protected readonly IConfiguration _configuration;
        protected readonly IMapper _mapper;
        protected readonly IPickupDatesService _pickupDatesService;

        public PickupDatesController(IConfiguration configuration, IMapper mapper, IPickupDatesService pickupDatesService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _pickupDatesService = pickupDatesService;
        }

        /// <summary>
        /// Returns the next pickup dates for an address
        /// </summary>
        /// <remarks>
        /// This returns the next garbage and recycling pickup dates for an address.
        /// </remarks>
        /// <returns></returns>
        [HttpGet("fromAddress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PickupDatesResultDTO>> GetPickupDates(string laddr, string sdir, string sname, string stype)
        {
            PickupDatesResults results = await _pickupDatesService.GetPickupDates(laddr, sdir, sname, stype);
            PickupDatesResultDTO dto = _mapper.Map<PickupDatesResultDTO>(results);

            return Ok(dto);
        }
    }
}
