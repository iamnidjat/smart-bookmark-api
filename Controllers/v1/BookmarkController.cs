using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBookmarkApi.Models;
using SmartBookmarkApi.Services.Interfaces;
using SmartBookmarkApi.Utilities;

namespace SmartBookmarkApi.Controllers.v1
{
    [Route("api/v1/[controller]/")]
    [ApiController]
    public class BookmarkController : ControllerBase
    {
        private readonly IBookmarkService _bookmarkService;
        public BookmarkController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookmarks = await _bookmarkService.GetAllAsync();
            return Ok(bookmarks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var bookmark = await _bookmarkService.GetByIdAsync(id);

            if (bookmark == null)
                return NotFound();

            return Ok(bookmark);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Bookmark bookmark)
        {
            var result = await _bookmarkService.AddAsync(bookmark);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Bookmark bookmark)
        {
            var result = await _bookmarkService.UpdateAsync(id, bookmark);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _bookmarkService.RemoveAsync(id);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string filterWord)
        {
            if (string.IsNullOrWhiteSpace(filterWord))
                return BadRequest("Filter word is required.");

            var result = await _bookmarkService.FilterBookmarks(filterWord);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPost("{bookmarkId}/visit")] // route parameter
        public async Task<IActionResult> RegisterVisit(int bookmarkId)
        {
            var result = await _bookmarkService.RegisterVisitAsync(bookmarkId);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPatch("{bookmarkId}/change-category")]
        public async Task<IActionResult> ChangeBookmarkCategory([FromRoute] int bookmarkId, [FromQuery] int newCategoryId)
        {
            var result = await _bookmarkService.ChangeBookmarkCategory(bookmarkId, newCategoryId);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }
    }
}
