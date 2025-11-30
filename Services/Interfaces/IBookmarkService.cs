using SmartBookmarkApi.Models;
using SmartBookmarkApi.Utilities;

namespace SmartBookmarkApi.Services.Interfaces
{
    public interface IBookmarkService
    {
        Task<List<Bookmark>> GetAllAsync();
        Task<Bookmark?> GetByIdAsync(int id);
        Task<OperationResult> AddAsync(Bookmark bookmark);
        Task<OperationResult> UpdateAsync(int id, Bookmark bookmark);
        Task<OperationResult> RemoveAsync(int id);
    }
}
