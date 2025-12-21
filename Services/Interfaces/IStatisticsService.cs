using SmartBookmarkApi.DTOs;

namespace SmartBookmarkApi.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<List<BookmarkVisitCountDto>> GetMostVisitedAsync(DateTime from, int take);
    }
}
