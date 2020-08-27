using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MkeTools.Web.Exceptions;
using MkeTools.Web.Middleware.Exceptions;
using MkeTools.Web.Models.Data;
using MkeTools.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Controllers
{
    [ApiController]
    public abstract class RelationshipWriteController<TDataModel, TDTOModel, TService> : RelationshipReadController<TDataModel, TDTOModel, TService>
    where TDataModel : class
    where TDTOModel : class
    where TService : IRelationshipReadService<TDataModel>, IRelationshipWriteService<TDataModel>
    {
        protected readonly IRelationshipWriteService<TDataModel> _writeService;

        public RelationshipWriteController(IConfiguration configuration, IMapper mapper, TService service) : base(configuration, mapper, service)
        {
            _writeService = service;
        }

        /// <summary>
        /// Creates a new item
        /// </summary>
        /// <param name="parentId">The ID of the parent of the item to create</param>
        /// <param name="dtoModel">The item to create</param>
        /// <returns></returns>
        /// <response code="400">Item validation failed</response>
        /// <response code="403">User does not have permission to create item</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TDTOModel>> Create(Guid parentId, [FromBody] TDTOModel dtoModel)
        {
            TDataModel dataModel = _mapper.Map<TDTOModel, TDataModel>(dtoModel);

            dataModel = await _writeService.Create(HttpContext.User, parentId, dataModel);

            return CreatedAtAction(nameof(GetOne), new { parentId = parentId, id = _writeService.GetChildId(dataModel) }, dataModel);
        }

        /// <summary>
        /// Updates an item
        /// </summary>
        /// <param name="parentId">The ID of the parent of the item to update</param>
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
        public async Task<ActionResult> Update(Guid parentId, Guid id, [FromBody] TDTOModel dtoModel)
        {
            TDataModel dataModel = _mapper.Map<TDTOModel, TDataModel>(dtoModel);

            if (id != _writeService.GetChildId(dataModel))
                throw new InvalidRequestException("Item ID in the URL did not match the item ID in the body");
            if (parentId != _writeService.GetParentId(dataModel))
                throw new InvalidRequestException("Parent ID in the URL did not match the parent ID in the body");

            await _writeService.Update(HttpContext.User, parentId, dataModel);

            TDTOModel returnValue = _mapper.Map<TDataModel, TDTOModel>(dataModel);

            return Ok(returnValue);
        }

        /// <summary>
        /// Deletes an item
        /// </summary>
        /// <param name="parentId">The parent ID of the item to delete</param>
        /// <param name="id">The ID of the item to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(Guid parentId, Guid id)
        {
            await _writeService.Delete(HttpContext.User, parentId, id);

            return NoContent();
        }
    }
}
