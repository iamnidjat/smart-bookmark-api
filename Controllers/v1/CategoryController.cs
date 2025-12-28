using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmartBookmarkApi.Models;
using SmartBookmarkApi.Services.Implementations;
using SmartBookmarkApi.Services.Interfaces;

namespace SmartBookmarkApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetAllAsync(cancellationToken);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetByIdAsync(id, cancellationToken);

            if (!result.Success)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category, CancellationToken cancellationToken)
        {
            var result = await _categoryService.AddAsync(category, cancellationToken);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Created("", result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category, CancellationToken cancellationToken)
        {
            var result = await _categoryService.UpdateAsync(id, category, cancellationToken);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id, CancellationToken cancellationToken)
        {
            var result = await _categoryService.RemoveAsync(id, cancellationToken);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string filterWord, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(filterWord))
                return BadRequest("Filter word is required.");

            var result = await _categoryService.FilterCategories(filterWord, cancellationToken);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }
    }
}
