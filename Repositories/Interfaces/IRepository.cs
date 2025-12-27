using SmartBookmarkApi.Utilities;

namespace SmartBookmarkApi.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<OperationResultOfT<T>> AddAsync(T entity, CancellationToken cancellationToken);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<OperationResult> UpdateAsync(int id, T updatedEntity, CancellationToken cancellationToken);
        Task<OperationResult> RemoveAsync(int id, CancellationToken cancellationToken);
    }
}
