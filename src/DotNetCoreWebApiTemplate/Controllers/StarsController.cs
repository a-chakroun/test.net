// -----------------------------------------------------------
// <copyright file="StarsController.cs" company="Company Name">
// Copyright YEAR COPYRIGHT HOLDER
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall
// be included in all copies or substantial portions of the
// Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Sample API controller.
// </summary>
// -----------------------------------------------------------

namespace DotNetCoreWebApiTemplate.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DotNetCoreWebApiTemplate.Models;
    using DotNetCoreWebApiTemplate.Storage;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StarsController : Controller
    {
        private const string GetStarRouteName = "GetStar";
        private readonly StarsStorage starsStorage;
        private readonly ILogger<StarsController> logger;

        public StarsController(StarsStorage starsStorage, ILogger<StarsController> logger)
        {
            this.starsStorage = starsStorage;
            this.logger = logger;
        }

        /// <summary>
        /// Get all stars
        /// </summary>
        /// <returns>200 on success with list of <see cref="Star"/></returns>
        /// <response code="200">When stars are returned</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Star>), 200)]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("Received request to retrieve all stars");

            return Ok(await starsStorage.GetAllAsync());
        }

        /// <summary>
        /// Get star by ID
        /// </summary>
        /// <param name="id">ID of star to return</param>
        /// <returns>200 on success with <see cref="Star"/></returns>
        /// <response code="200">When the star is returned</response>
        /// <response code="404">When the star is not found</response>
        [HttpGet("{id}", Name = GetStarRouteName)]
        [ProducesResponseType(typeof(Star), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(Guid id)
        {
            logger.LogInformation("Received request to retrieve star {ID}", id);

            try
            {
                return Ok(await starsStorage.GetAsync(id));
            }
            catch (KeyNotFoundException)
            {
                logger.LogWarning("Received request to retrieve non existent star {ID}", id);

                return NotFound();
            }
        }

        /// <summary>
        /// Add new star
        /// </summary>
        /// <param name="star">The star to add</param>
        /// <returns>201 on success with the <see cref="Star"/> containing the assigned ID</returns>
        /// <response code="201">When the star is added</response>
        /// <response code="400">When the star is invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(Star), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Add([FromBody] Star star)
        {
            logger.LogInformation("Received request to create star {Name}", star.Name);

            try
            {
                await starsStorage.AddAsync(star);

                return CreatedAtRoute(GetStarRouteName, new { id = star.Id }, star);
            }
            catch (ArgumentException)
            {
                logger.LogWarning("Received request to create a new star but an ID was supplied {ID}", star.Id);

                return BadRequest(new ErrorResponse("Star ID must be empty to create."));
            }
        }

        /// <summary>
        /// Update existing star
        /// </summary>
        /// <param name="id">ID of star to update</param>
        /// <param name="star">The star to update</param>
        /// <returns>204 on success</returns>
        /// <response code="204">When the star is updated</response>
        /// <response code="404">When the star is not found</response>
        /// <response code="400">When the star is invalid</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Update(Guid id, [FromBody] Star star)
        {
            logger.LogInformation("Received request to update star {ID}", id);

            if (id != star.Id)
            {
                logger.LogWarning("Received request to update a star but the ID in the URL {ID} did not match the model ID {ModelID}", id, star.Id);

                return BadRequest(new ErrorResponse("Star ID in URL must match ID in model."));
            }

            try
            {
                await starsStorage.UpdateAsync(star);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                logger.LogWarning("Received request to update non existent star {ID}", id);

                return NotFound();
            }
        }

        /// <summary>
        /// Delete existing star
        /// </summary>
        /// <param name="id">ID of star to delete</param>
        /// <returns>204 on success</returns>
        /// <response code="204">When the star is deleted</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(Guid id)
        {
            logger.LogInformation("Received request to delete star {ID}", id);

            try
            {
                await starsStorage.DeleteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                logger.LogWarning("Received request to delete non existent star {ID}", id);
            }

            return NoContent();
        }
    }
}