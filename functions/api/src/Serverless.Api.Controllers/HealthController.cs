// ***********************************************************************
// <copyright file="HealthController.cs" company="GBM">
//     Serverless example
// </copyright>
// ***********************************************************************

using Serverless.Api.Common.Constants;
using Microsoft.AspNetCore.Mvc;

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
        /// <response code="200">The service health check.</response>
        /// <response code="500">An unhandled error in the server.</response>
        [HttpGet(Name = "GetHealth")]
        public async Task<IActionResult> GetHealthAsync()
        {
            var result = await Task.FromResult("healthy").ConfigureAwait(false);
            return this.Ok(result);

        }
    }
}
