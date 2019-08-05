using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Exceptions;
using MkeAlerts.Web.Middleware.Exceptions;
using MkeAlerts.Web.Models.Data;
using MkeAlerts.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class EntityWriteController<TDataModel, TDTOModel, TService, TIdType> : EntityReadController<TDataModel, TDTOModel, TService, TIdType>
    where TDataModel : class, IHasId<TIdType>
    where TDTOModel : class
    where TService : IEntityReadService<TDataModel, TIdType>, IEntityWriteService<TDataModel, TIdType>
    {
        protected readonly IEntityWriteService<TDataModel, TIdType> _writeService;

        public EntityWriteController(IConfiguration configuration, IMapper mapper, TService service) : base(configuration, mapper, service)
        {
            _writeService = service;
        }

        /// <summary>
        /// Creates a new item
        /// </summary>
        /// <param name="dtoModel">The item to create</param>
        /// <returns></returns>
        /// <response code="400">Item validation failed</response>
        /// <response code="403">User does not have permission to create item</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TDTOModel>> Create([FromBody] TDTOModel dtoModel)
        {
            TDataModel dataModel = _mapper.Map<TDTOModel, TDataModel>(dtoModel);

            dataModel = await _writeService.Create(HttpContext.User, dataModel);

            return CreatedAtAction(nameof(GetOne), new { id = dataModel.GetId() }, dataModel);
        }

        /// <summary>
        /// Updates an item
        /// </summary>
        /// <param name="id">The ID of the item to update</param>
        /// <param name="dtoModel">The new value for the item</param>
        /// <returns>Returns the item (with an updated timestamp)</returns>
        /// <response code="400">Item validation failed</response>
        /// <response code="403">User does not have permission to update item</response>
        /// <response code="409">Concurrency conflict. The item sent in the request is no longer the most recent version of the item</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(TIdType id, [FromBody] TDTOModel dtoModel)
        {
            TDataModel dataModel = _mapper.Map<TDTOModel, TDataModel>(dtoModel);

            await _writeService.Update(HttpContext.User, dataModel);

            TDTOModel returnValue = _mapper.Map<TDataModel, TDTOModel>(dataModel);

            return Ok(returnValue);
        }

        /// <summary>
        /// Deletes an item
        /// </summary>
        /// <param name="id">The ID of the item to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(TIdType id)
        {
            await _writeService.Delete(HttpContext.User, id);

            return NoContent();
        }
    }
}
