using System.ComponentModel.DataAnnotations;
using HackerNews.Contract;
using HackerNews.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StoriesController : ControllerBase
    {
        private readonly IBestStoriesService _bestStoryService;
        private const int MaxNumberOfStories = 100;

        public StoriesController(IBestStoriesService bestStoryService)
        {
            _bestStoryService = bestStoryService;
        }

        /// <summary>
        /// Retrieves the top Hacker News stories.
        /// </summary>
        /// <param name="count">Number of top stories to return. Must be between 1 and 100. Defaults to 10.</param>
        /// <returns>List of top stories.</returns>
        /// <response code="200">Returns the list of stories.</response>
        /// <response code="400">If the request is invalid (e.g. count out of range).</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet("best")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Story>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBestStories([FromQuery][Range(1, MaxNumberOfStories)] int count = 10)
        {
            if (count <= 0 || count > MaxNumberOfStories)
            {
                return BadRequest($"n must be between 1 and {MaxNumberOfStories}");
            }

            try
            {
                var stories = await _bestStoryService.GetBestStoriesAsync(count);
                return Ok(stories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
