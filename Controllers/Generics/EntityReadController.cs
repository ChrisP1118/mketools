using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Exceptions;
using MkeAlerts.Web.Filters.Support;
using MkeAlerts.Web.Middleware.Exceptions;
using MkeAlerts.Web.Models.Data;
using MkeAlerts.Web.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MkeAlerts.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class EntityReadController<TDataModel, TDTOModel, TService, TIdType> : ControllerBase
        where TDataModel : class, IHasId<TIdType>
        where TDTOModel : class
        where TService : IEntityReadService<TDataModel, TIdType>
    {
        protected readonly IConfiguration _configuration;
        protected readonly IMapper _mapper;
        protected readonly IEntityReadService<TDataModel, TIdType> _readService;

        public EntityReadController(IConfiguration configuration, IMapper mapper, TService readService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _readService = readService;
        }

        /// <summary>
        /// Returns all items
        /// </summary>
        /// <param name="offset">The first offset in the result set to return</param>
        /// <param name="limit">The maximum number of results to return</param>
        /// <param name="order">The order in which to sort results</param>
        /// <param name="filter">The filters to apply to the results</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-Total-Count", "int", "Returns the total number of available items")]
        public async Task<ActionResult<IEnumerable<TDTOModel>>> GetAllAsync(int offset = 0, int limit = 10, string order = null, string filter = null, double? northBound = null, double? southBound = null, double? eastBound = null, double? westBound = null)
        {
            List<TDataModel> dataModelItems = await _readService.GetAll(HttpContext.User, offset, limit, order, filter, northBound, southBound, eastBound, westBound, null);

            List<TDTOModel> dtoModelItems = dataModelItems
                .Select(d => _mapper.Map<TDataModel, TDTOModel>(d))
                .ToList();

            long count = await _readService.GetAllCount(HttpContext.User, filter, northBound, southBound, eastBound, westBound);
            Response.Headers.Add("X-Total-Count", count.ToString());

            return Ok(dtoModelItems);
        }

        /// <summary>
        /// Returns a single item
        /// </summary>
        /// <param name="id">The ID of the item to return</param>
        /// <returns></returns>
        [HttpGet("{*id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<TDTOModel>> GetOne(TIdType id)
        {
            TDataModel dataModel = await _readService.GetOne(HttpContext.User, id);

            if (dataModel == null)
                return NotFound();

            TDTOModel dtoModel = _mapper.Map<TDataModel, TDTOModel>(dataModel);

            return Ok(dtoModel);
        }
    }
}