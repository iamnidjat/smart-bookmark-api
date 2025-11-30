using SmartBookmarkApi.Utilities;

namespace SmartBookmarkApi.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<OperationResult> AddAsync(T entity);
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<OperationResult> UpdateAsync(int id, T updatedEntity);
        Task<OperationResult> RemoveAsync(int id);
    }
}
