using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Filters.Support;
using MkeAlerts.Web.Middleware.Exceptions;
using MkeAlerts.Web.Models.Data.Places;
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
    public class StreetReferenceController : ControllerBase
    {
        protected readonly IConfiguration _configuration;
        protected readonly IMapper _mapper;
        protected readonly IStreetReferenceService _streetReferenceService;

        public StreetReferenceController(IConfiguration configuration, IMapper mapper, IStreetReferenceService streetReferenceService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _streetReferenceService = streetReferenceService;
        }

        /// <summary>
        /// Returns all street directions
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<string>>> GetAllAsync()
        {
            // TODO: This could benefit from some parallelism
            // TODO: All of these should be cached as well

            List<string> streetDirections = await _streetReferenceService.GetAllStreetDirections(HttpContext.User);
            List<string> streetNames = await _streetReferenceService.GetAllStreetNames(HttpContext.User);
            List<string> streetTypes = await _streetReferenceService.GetAllStreetTypes(HttpContext.User);

            return Ok(new
            {
                streetDirections = streetDirections,
                streetNames = streetNames,
                streetTypes = streetTypes
            });
        }

        /// <summary>
        /// Returns all street directions
        /// </summary>
        /// <returns></returns>
        [HttpGet("StreetDirections")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<string>>> GetAllStreetDirectionsAsync()
        {
            var items = await _streetReferenceService.GetAllStreetDirections(HttpContext.User);

            return Ok(items);
        }

        /// <summary>
        /// Returns all street names
        /// </summary>
        /// <returns></returns>
        [HttpGet("StreetNames")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<string>>> GetAllStreetNamesAsync()
        {
            var items = await _streetReferenceService.GetAllStreetNames(HttpContext.User);

            return Ok(items);
        }

        /// <summary>
        /// Returns all street types
        /// </summary>
        /// <returns></returns>
        [HttpGet("StreetTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<string>>> GetAllStreetTypesAsync()
        {
            var items = await _streetReferenceService.GetAllStreetTypes(HttpContext.User);

            return Ok(items);
        }

    }
}
