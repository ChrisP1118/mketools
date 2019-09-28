﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Filters.Support;
using MkeAlerts.Web.Middleware.Exceptions;
using MkeAlerts.Web.Models.Data.Places;
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
    public class GeocodingController : ControllerBase
    {
        protected readonly IConfiguration _configuration;
        protected readonly IMapper _mapper;
        protected readonly IGeocodingService _geocodingService;

        public GeocodingController(IConfiguration configuration, IMapper mapper, IGeocodingService geocodingService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _geocodingService = geocodingService;
        }

        /// <summary>
        /// Geocodes a string
        /// </summary>
        /// <remarks>
        /// The preferred syntax is: NUMBER DIRECTION STREET STREET-TYPE, such as: 200 E WELLS ST
        /// 
        /// This can also be used for intersections. Put a slash between the street names, like: E WELLS ST / N WATER ST
        /// </remarks>
        /// <returns></returns>
        [HttpGet("FromAddress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<string>>> GetLocation(string address)
        {
            GeocodeResults results = await _geocodingService.Geocode(address);

            return Ok(results);
        }

        /// <summary>
        /// Reverse geocodes a string to the nearest address
        /// </summary>
        /// <remarks>
        /// This will return an address and a distance from the address to the provided coordinates. Intersections are not returned, only addresses
        /// </remarks>
        /// <returns></returns>
        [HttpGet("FromCoordinates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Address>> GetAddress(double latitude, double longitude)
        {
            ReverseGeocodeResults results = await _geocodingService.ReverseGeocode(latitude, longitude);

            return Ok(results);
        }
    }
}