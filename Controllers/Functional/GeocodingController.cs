using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MkeTools.Web.Filters.Support;
using MkeTools.Web.Middleware.Exceptions;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Models.DTO.Geocoding;
using MkeTools.Web.Models.Internal;
using MkeTools.Web.Services;
using MkeTools.Web.Services.Data;
using MkeTools.Web.Services.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Controllers.Functional
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
        [HttpGet("fromAddress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<GeocodeResultsDTO>> GetLocation(string address)
        {
            GeocodeResults results = await _geocodingService.Geocode(address);
            GeocodeResultsDTO dto = _mapper.Map<GeocodeResultsDTO>(results);

            return Ok(dto);
        }

        /// <summary>
        /// Reverse geocodes a string to the nearest address
        /// </summary>
        /// <remarks>
        /// This will return an address and a distance from the address to the provided coordinates. Intersections are not returned, only addresses
        /// </remarks>
        /// <returns></returns>
        [HttpGet("fromCoordinates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ReverseGeocodeResultsDTO>> GetAddress(double latitude, double longitude)
        {
            ReverseGeocodeResults results = await _geocodingService.ReverseGeocode(latitude, longitude);
            ReverseGeocodeResultsDTO dto = _mapper.Map<ReverseGeocodeResultsDTO>(results);

            return Ok(dto);
        }
    }
}
