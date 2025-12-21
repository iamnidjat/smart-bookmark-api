using SmartBookmarkApi.Data;
using SmartBookmarkApi.DTOs;
using SmartBookmarkApi.Models;
using SmartBookmarkApi.Repositories.Interfaces;
using SmartBookmarkApi.Services.Interfaces;
using SmartBookmarkApi.Utilities;

namespace SmartBookmarkApi.Services.Implementations
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IBookmarkVisitRepository _bookmarkVisitRepository;
        private readonly AppDbContext _context;
        private readonly ILogger<BookmarkService> _logger;

        public StatisticsService(IBookmarkVisitRepository bookmarkVisitRepository,
            AppDbContext context,
            ILogger<BookmarkService> logger
            )
        {
            _context = context;
            _logger = logger;
            _bookmarkVisitRepository = bookmarkVisitRepository;
        }

        public async Task<OperationResultOfT<List<BookmarkVisitCountDto>>> GetMostVisitedAsync(DateTime from, int take)
        {
            try
            {
                var mostVisited = await _bookmarkVisitRepository.GetMostVisitedAsync(from, take);

                return new OperationResultOfT<List<BookmarkVisitCountDto>>
                {
                    Success = true,
                    Data = mostVisited
                };
            }
            catch (Exception ex) when (ex is OperationCanceledException or ArgumentNullException)
            {
                _logger.LogError(ex, "Failed to get most visited bookmarks");
                return new OperationResultOfT<List<BookmarkVisitCountDto>> { Success = false, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting most visited bookmarks.");
                return new OperationResultOfT<List<BookmarkVisitCountDto>> { Success = false, ErrorMessage = ex.Message };
            }
        }

        Task<List<BookmarkVisitCountDto>> IStatisticsService.GetMostVisitedAsync(DateTime from, int take)
        {
            throw new NotImplementedException();
        }
    }
}
