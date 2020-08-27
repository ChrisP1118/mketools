using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MkeTools.Web.Exceptions;
using MkeTools.Web.Models.Data.Places;
using MkeTools.Web.Models.Data.Subscriptions;
using MkeTools.Web.Models.DTO.Places;
using MkeTools.Web.Models.DTO.Subscriptions;
using MkeTools.Web.Services;
using MkeTools.Web.Utilities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeTools.Web.Controllers.Data
{
    [Authorize]
    public class PickupDatesSubscriptionController : EntityWriteController<PickupDatesSubscription, PickupDatesSubscriptionDTO, IEntityWriteService<PickupDatesSubscription, Guid>, Guid>
    {
        public PickupDatesSubscriptionController(IConfiguration configuration, IMapper mapper, IEntityWriteService<PickupDatesSubscription, Guid> service) : base(configuration, mapper, service)
        {
        }

        /// <summary>
        /// Unsubscribes from a pickup dates subscription
        /// </summary>
        /// <param name="model"></param>
        [AllowAnonymous]
        [HttpPost("unsubscribe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PickupDatesSubscriptionDTO>> Unsubscribe([FromBody] UnsubscribePickupDatesSubscriptionDTO model)
        {
            string validHash = EncryptionUtilities.GetHash(model.SubscriptionId.ToString() + ":" + model.ApplicationUserId.ToString(), _configuration["HashKey"]);
            if (model.Hash != validHash)
                throw new InvalidRequestException("The hash was invalid.");

            ClaimsPrincipal claimsPrincipal = UserUtilities.GetClaimsPrincipal(model.ApplicationUserId);

            await _writeService.Delete(claimsPrincipal, model.SubscriptionId);

            return NoContent();
        }
    }
}
