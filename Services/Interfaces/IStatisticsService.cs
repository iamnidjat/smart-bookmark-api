using SmartBookmarkApi.DTOs;
using SmartBookmarkApi.Utilities;

namespace SmartBookmarkApi.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<OperationResultOfT<List<BookmarkVisitCountDto>>> GetMostVisitedAsync(DateTime from, int take, CancellationToken cancellationToken);
    }
}
