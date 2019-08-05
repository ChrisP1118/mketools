using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Jobs;
using MkeAlerts.Web.Models.Data.Properties;
using MkeAlerts.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Controllers.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        protected readonly IConfiguration _configuration;
        protected readonly IMapper _mapper;

        protected readonly IEntityWriteService<Property, string> _propertyWriteService;

        public JobsController(
            IConfiguration configuration, 
            IMapper mapper,
            IEntityWriteService<Property, string> propertyWriteService)
        {
            _configuration = configuration;
            _mapper = mapper;

            _propertyWriteService = propertyWriteService;
        }

        /// <summary>
        /// Imports properties
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ImportProperties()
        {
            ImportPropertiesJob job = new ImportPropertiesJob(_propertyWriteService);
            await job.Run(HttpContext.User);

            return Ok();
        }
    }
}
