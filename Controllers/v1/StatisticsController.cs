using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBookmarkApi.Extensions;
using SmartBookmarkApi.Services.Implementations;
using SmartBookmarkApi.Services.Interfaces;

namespace SmartBookmarkApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;
        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("most-visited")]
        public async Task<IActionResult> GetMostVisited([FromQuery] DateTime from, [FromQuery] int take, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var bookmarks = await _statisticsService.GetMostVisitedAsync(userId, from, take, cancellationToken);

            if (bookmarks == null)
                return NotFound();

            return Ok(bookmarks);
        }

    }
}
