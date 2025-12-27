using SmartBookmarkApi.Models;
using SmartBookmarkApi.Utilities;

namespace SmartBookmarkApi.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync(CancellationToken cancellationToken);
        Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<OperationResultOfT<Category>> AddAsync(Category category, CancellationToken cancellationToken);
        Task<OperationResult> UpdateAsync(int id, Category category, CancellationToken cancellationToken);
        Task<OperationResult> RemoveAsync(int id, CancellationToken cancellationToken);
        Task<OperationResultOfT<List<Category>>> FilterCategories(string filterWord, CancellationToken cancellationToken);
    }
}
