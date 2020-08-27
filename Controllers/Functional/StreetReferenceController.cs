using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MkeTools.Web.Filters.Support;
using MkeTools.Web.Middleware.Exceptions;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Models.DTO.Functional;
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
        /// Returns all street directions, names, and types
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<StreetReferenceDTO>> GetAllAsync(string municipality)
        {
            // TODO: This could benefit from some parallelism
            // TODO: All of these should be cached as well

            List<string> streetDirections = await _streetReferenceService.GetAllStreetDirections(HttpContext.User, municipality);
            List<string> streetNames = await _streetReferenceService.GetAllStreetNames(HttpContext.User, municipality);
            List<string> streetTypes = await _streetReferenceService.GetAllStreetTypes(HttpContext.User, municipality);

            return Ok(new StreetReferenceDTO
            {
                StreetDirections = streetDirections,
                StreetNames = streetNames,
                StreetTypes = streetTypes
            });
        }

        /// <summary>
        /// Returns all street directions
        /// </summary>
        /// <returns></returns>
        [HttpGet("streetDirections")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<string>>> GetAllStreetDirectionsAsync(string municipality)
        {
            var items = await _streetReferenceService.GetAllStreetDirections(HttpContext.User, municipality);

            return Ok(items);
        }

        /// <summary>
        /// Returns all street names
        /// </summary>
        /// <returns></returns>
        [HttpGet("streetNames")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<string>>> GetAllStreetNamesAsync(string municipality)
        {
            var items = await _streetReferenceService.GetAllStreetNames(HttpContext.User, municipality);

            return Ok(items);
        }

        /// <summary>
        /// Returns all street types
        /// </summary>
        /// <returns></returns>
        [HttpGet("streetTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<string>>> GetAllStreetTypesAsync(string municipality)
        {
            var items = await _streetReferenceService.GetAllStreetTypes(HttpContext.User, municipality);

            return Ok(items);
        }

    }
}
