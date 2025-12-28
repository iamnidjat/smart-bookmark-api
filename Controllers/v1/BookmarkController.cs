using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmartBookmarkApi.Models;
using SmartBookmarkApi.Services.Interfaces;
using SmartBookmarkApi.Utilities;

namespace SmartBookmarkApi.Controllers.v1
{
    [Route("api/v1/[controller]/")]
    [ApiController] 
    [Authorize] // Allows only authenticated users with a valid JWT token to access these endpoints
    public class BookmarkController : ControllerBase
    {
        private readonly IBookmarkService _bookmarkService;
        public BookmarkController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _bookmarkService.GetAllAsync(cancellationToken);
            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await _bookmarkService.GetByIdAsync(id, cancellationToken);

            if (!result.Success || result.Data == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Bookmark bookmark, CancellationToken cancellationToken)
        {
            var result = await _bookmarkService.AddAsync(bookmark, cancellationToken);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Created("", result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Bookmark bookmark, CancellationToken cancellationToken)
        {
            var result = await _bookmarkService.UpdateAsync(id, bookmark, cancellationToken);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id, CancellationToken cancellationToken)
        {
            var result = await _bookmarkService.RemoveAsync(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string filterWord, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(filterWord))
                return BadRequest("Filter word is required.");

            var result = await _bookmarkService.FilterBookmarks(filterWord, cancellationToken);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPost("{bookmarkId}/visit")] // route parameter
        public async Task<IActionResult> RegisterVisit(int bookmarkId, CancellationToken cancellationToken)
        {
            var result = await _bookmarkService.RegisterVisitAsync(bookmarkId, cancellationToken);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPatch("{bookmarkId}/change-category")]
        public async Task<IActionResult> ChangeBookmarkCategory([FromRoute] int bookmarkId, [FromQuery] int newCategoryId, CancellationToken cancellationToken)
        {
            var result = await _bookmarkService.ChangeBookmarkCategory(bookmarkId, newCategoryId, cancellationToken);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }
    }
}
