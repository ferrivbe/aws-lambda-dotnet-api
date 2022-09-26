// ***********************************************************************
// <copyright file="HealthController.cs">
//     Serverless example
// </copyright>
// ***********************************************************************

using Serverless.Api.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Serverless.Api.Models.Models;

namespace Serverless.Api.Controllers
{
    /// <summary>
    /// HAndles all requests for service health
    /// </summary>
    [ApiController]
    [Route(Routes.Health)]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Adds the specified address.
        /// </summary>
        /// <returns>The service health check.</returns>
        /// <returns>A newly created TodoItem</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /health
        ///
        /// </remarks>
        /// <response code="200">The service health check.</response>
        [HttpGet]
        public async Task<IActionResult> GetHealthAsync()
        {
            var response = new HealthDto
            {
                ServiceHealth = "healthy"
            };

            var result = await Task.FromResult(response).ConfigureAwait(false);
            return this.Ok(result);

        }
    }
}
