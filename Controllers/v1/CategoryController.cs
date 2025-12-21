using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBookmarkApi.Models;
using SmartBookmarkApi.Services.Implementations;
using SmartBookmarkApi.Services.Interfaces;

namespace SmartBookmarkApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookmarks = await _categoryService.GetAllAsync();
            return Ok(bookmarks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var bookmark = await _categoryService.GetByIdAsync(id);

            if (bookmark == null)
                return NotFound();

            return Ok(bookmark);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            var result = await _categoryService.AddAsync(category);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            var result = await _categoryService.UpdateAsync(id, category);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _categoryService.RemoveAsync(id);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string filterWord)
        {
            if (string.IsNullOrWhiteSpace(filterWord))
                return BadRequest("Filter word is required.");

            var result = await _categoryService.FilterCategories(filterWord);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }
    }
}
