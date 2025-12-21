using Microsoft.EntityFrameworkCore;
using SmartBookmarkApi.Data;
using SmartBookmarkApi.Models;
using SmartBookmarkApi.Models.Base;
using SmartBookmarkApi.Repositories.Interfaces;
using SmartBookmarkApi.Services.Interfaces;
using SmartBookmarkApi.Utilities;

namespace SmartBookmarkApi.Services.Implementations
{
    // BookmarkService handles all business logic related to bookmarks.
    // It uses the BookmarkRepository to interact with the database,
    // but adds validation, authorization checks, and processing.
    // Controllers call the service layer instead of directly using repositories.

    public class BookmarkService : IBookmarkService
    {
        private readonly IBookmarkRepository _bookmarkRepository;
        private readonly IBookmarkVisitRepository _bookmarkVisitRepository;
        private readonly AppDbContext _context;
        private readonly ILogger<BookmarkService> _logger;

        public BookmarkService(IBookmarkRepository bookmarkRepository,
            IBookmarkVisitRepository bookmarkVisitRepository,
            AppDbContext context,
            ILogger<BookmarkService> logger
            )
        {
            _bookmarkRepository = bookmarkRepository;
            _context = context;
            _logger = logger;
            _bookmarkVisitRepository = bookmarkVisitRepository;
        }

        public async Task<List<Bookmark>> GetAllAsync()
        {
            return await _bookmarkRepository.GetAllAsync();
        }

        public async Task<Bookmark?> GetByIdAsync(int id)
        {
            return await _bookmarkRepository.GetByIdAsync(id);
        }

        public async Task<OperationResult> AddAsync(Bookmark bookmark)
        {
            return await _bookmarkRepository.AddAsync(bookmark);
        }

        public async Task<OperationResult> UpdateAsync(int id, Bookmark bookmark)
        {
            return await _bookmarkRepository.UpdateAsync(id, bookmark);
        }

        public async Task<OperationResult> RemoveAsync(int id)
        {
            return await _bookmarkRepository.RemoveAsync(id);
        }

        public async Task<OperationResultOfT<List<Bookmark>>> FilterBookmarks(string filterWord)
        {
            try
            {
                var filteredBookmarks = await _bookmarkRepository.FilterAsync(filterWord);

                return new OperationResultOfT<List<Bookmark>>
                {
                    Success = true,
                    Data = filteredBookmarks
                };
            }
            catch (Exception ex) when (ex is OperationCanceledException or ArgumentNullException)
            {
                _logger.LogError(ex, "Failed to filter bookmarks");
                return new OperationResultOfT<List<Bookmark>> { Success = false, ErrorMessage = ex.Message };
            }
            catch (Exception ex) // general fallback for unexpected exceptions
            {
                _logger.LogError(ex, "Unexpected error occurred while filtering bookmarks.");
                return new OperationResultOfT<List<Bookmark>> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<OperationResult> RegisterVisitAsync(int bookmarkId)
        {
            try
            {
                await _bookmarkVisitRepository.RegisterVisitAsync(bookmarkId);

                return new OperationResult
                {
                    Success = true
                };
            }
            catch (Exception ex) when (ex is DbUpdateConcurrencyException or DbUpdateException or OperationCanceledException)
            {
                _logger.LogError(ex, "Failed to register visit");
                return new OperationResult { Success = false, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while registering visit.");
                return new OperationResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<OperationResult> ChangeBookmarkCategory(int bookmarkId, int newCategoryId)
        {
            try
            {
                var bookmark = await _bookmarkRepository.GetByIdAsync(bookmarkId);

                if (bookmark == null)
                {
                    return new OperationResult
                    {
                        Success = false,
                        ErrorMessage = "Bookmark not found."
                    };
                }

                bookmark.CategoryId = newCategoryId;
                await _context.SaveChangesAsync();

                return new OperationResult
                {
                    Success = true
                };
            }
            catch (Exception ex) when (ex is DbUpdateConcurrencyException or DbUpdateException or OperationCanceledException)
            {
                _logger.LogError(ex, "Failed to change the category of the {bookmark}", bookmarkId);
                return new OperationResult { Success = false, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while changing the category of the {bookmark}", bookmarkId);
                return new OperationResult { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
