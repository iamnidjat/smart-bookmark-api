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

        public async Task<OperationResultOfT<Category>> AddAsync(Category category, CancellationToken cancellationToken)
        {
            return await _categoryRepository.AddAsync(category, cancellationToken);
        }

        public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _categoryRepository.GetAllAsync(cancellationToken);
        }

        public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _categoryRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<OperationResult> RemoveAsync(int id, CancellationToken cancellationToken)
        {
            return await _categoryRepository.RemoveAsync(id, cancellationToken);
        }

        public async Task<OperationResult> UpdateAsync(int id, Category category, CancellationToken cancellationToken)
        {
            return await _categoryRepository.UpdateAsync(id, category, cancellationToken);
        }

        public async Task<OperationResultOfT<List<Category>>> FilterCategories(string filterWord, CancellationToken cancellationToken)
        {
            try
            {
                var filteredCategories = await _categoryRepository.FilterAsync(filterWord, cancellationToken);

                return new OperationResultOfT<List<Category>>
                {
                    Success = true,
                    Data = filteredCategories
                };
            }
            catch (Exception ex) when (ex is ArgumentNullException)
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
