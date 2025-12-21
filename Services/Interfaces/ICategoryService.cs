using SmartBookmarkApi.Models;
using SmartBookmarkApi.Utilities;

namespace SmartBookmarkApi.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<OperationResult> AddAsync(Category category);
        Task<OperationResult> UpdateAsync(int id, Category category);
        Task<OperationResult> RemoveAsync(int id);
        Task<OperationResultOfT<List<Category>>> FilterCategories(string filterWord);
    }
}
