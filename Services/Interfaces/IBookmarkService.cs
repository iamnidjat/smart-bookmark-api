using SmartBookmarkApi.Models;
using SmartBookmarkApi.Utilities;

namespace SmartBookmarkApi.Services.Interfaces
{
    public interface IBookmarkService
    {
        Task<OperationResultOfT<List<Bookmark>>> GetAllAsync(CancellationToken cancellationToken);
        Task<OperationResultOfT<Bookmark>> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<OperationResultOfT<Bookmark>> AddAsync(Bookmark bookmark, CancellationToken cancellationToken);
        Task<OperationResult> UpdateAsync(int id, Bookmark bookmark, CancellationToken cancellationToken);
        Task<OperationResult> RemoveAsync(int id, CancellationToken cancellationToken);
        Task<OperationResultOfT<List<Bookmark>>> FilterBookmarks(string filterWord, CancellationToken cancellationToken);
        Task<OperationResult> RegisterVisitAsync(int bookmarkId, CancellationToken cancellationToken);
        Task<OperationResult> ChangeBookmarkCategory(int bookmarkId, int newCategoryId, CancellationToken cancellationToken);
    }
}
