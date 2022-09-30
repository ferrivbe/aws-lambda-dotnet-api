// ***********************************************************************
// <copyright file="HealthController.cs">
//     Serverless example
// </copyright>
// ***********************************************************************

using Serverless.Api.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Serverless.Api.Models.Models;
using Microsoft.AspNetCore.Http;
using System.Net;
using Serverless.Api.Models.Models.Error;

namespace Serverless.Api.Controllers
{
    /// <summary>
    /// Handles all requests for service health
    /// </summary>
    [ApiController]
    [Route(Routes.Health)]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Gets the current service health with a timestamp.
        /// </summary>
        /// <returns>The service health check.</returns>
        /// <response code="200">The service health check.</response>
        /// <response code="500">The service internal error.</response>
        [HttpGet]
        [Consumes(MimeTypes.ApplicationJson)]
        [Produces(MimeTypes.ApplicationJson)]
        [ProducesResponseType(typeof(HealthDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HttpErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHealthAsync()
        {
            var response = new HealthDto
            {
                ServiceHealth = Responses.HealthyService,
                Timestamp = DateTime.Now,
            };

            var result = await Task.FromResult(response).ConfigureAwait(false);
            return this.Ok(result);
        }
    }
}
