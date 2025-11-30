using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBookmarkApi.Models;
using SmartBookmarkApi.Services.Interfaces;

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
    }
}
