using SmartBookmarkApi.DTOs;
using SmartBookmarkApi.Models;
using SmartBookmarkApi.Utilities;

namespace SmartBookmarkApi.Repositories.Interfaces
{
    public interface IBookmarkVisitRepository
    {
        Task RegisterVisitAsync(int bookmarkId, CancellationToken cancellationToken);
        Task<List<BookmarkVisitCountDto>> GetMostVisitedAsync(int userId, DateTime from, int take, CancellationToken cancellationToken);
    }
}
