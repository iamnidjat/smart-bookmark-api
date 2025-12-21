using Microsoft.EntityFrameworkCore;
using SmartBookmarkApi.Data;
using SmartBookmarkApi.Models;
using SmartBookmarkApi.Repositories.Implementations;
using SmartBookmarkApi.Repositories.Interfaces;
using SmartBookmarkApi.Services.Interfaces;
using SmartBookmarkApi.Utilities;

namespace SmartBookmarkApi.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly AppDbContext _context;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
          ICategoryRepository categoryRepository,
          AppDbContext context,
          ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _context = context;
            _logger = logger;
        }

        public async Task<OperationResult> AddAsync(Category category)
        {
            return await _categoryRepository.AddAsync(category);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<OperationResult> RemoveAsync(int id)
        {
            return await _categoryRepository.RemoveAsync(id);
        }

        public async Task<OperationResult> UpdateAsync(int id, Category category)
        {
            return await _categoryRepository.UpdateAsync(id, category);
        }

        public async Task<OperationResultOfT<List<Category>>> FilterCategories(string filterWord)
        {
            try
            {
                var filteredCategories = await _categoryRepository.FilterAsync(filterWord);

                return new OperationResultOfT<List<Category>>
                {
                    Success = true,
                    Data = filteredCategories
                };
            }
            catch (Exception ex) when (ex is OperationCanceledException or ArgumentNullException)
            {
                _logger.LogError(ex, "Failed to filter bookmarks");
                return new OperationResultOfT<List<Category>> { Success = false, ErrorMessage = ex.Message };
            }
            catch (Exception ex) // general fallback for unexpected exceptions
            {
                _logger.LogError(ex, "Unexpected error occurred while filtering bookmarks.");
                return new OperationResultOfT<List<Category>> { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
