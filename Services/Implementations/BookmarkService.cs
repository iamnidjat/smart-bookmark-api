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

        public async Task<OperationResultOfT<List<Bookmark>>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _bookmarkRepository.GetAllAsync(cancellationToken);
        }

        public async Task<OperationResultOfT<Bookmark>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _bookmarkRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<OperationResultOfT<Bookmark>> AddAsync(Bookmark bookmark, CancellationToken cancellationToken)
        {
            return await _bookmarkRepository.AddAsync(bookmark, cancellationToken);
        }

        public async Task<OperationResult> UpdateAsync(int id, Bookmark bookmark, CancellationToken cancellationToken)
        {
            return await _bookmarkRepository.UpdateAsync(id, bookmark, cancellationToken);
        }

        public async Task<OperationResult> RemoveAsync(int id, CancellationToken cancellationToken)
        {
            return await _bookmarkRepository.RemoveAsync(id, cancellationToken);
        }

        public async Task<OperationResultOfT<List<Bookmark>>> FilterBookmarks(string filterWord, CancellationToken cancellationToken)
        {
            try
            {
                var filteredBookmarks = await _bookmarkRepository.FilterAsync(filterWord, cancellationToken);

                return new OperationResultOfT<List<Bookmark>>
                {
                    Success = true,
                    Data = filteredBookmarks
                };
            }
            catch (Exception ex) when (ex is ArgumentNullException)
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

        public async Task<OperationResult> RegisterVisitAsync(int bookmarkId, CancellationToken cancellationToken)
        {
            try
            {
                await _bookmarkVisitRepository.RegisterVisitAsync(bookmarkId, cancellationToken);

                return new OperationResult
                {
                    Success = true
                };
            }
            catch (Exception ex) when (ex is DbUpdateConcurrencyException or DbUpdateException)
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

        public async Task<OperationResult> ChangeBookmarkCategory(int bookmarkId, int newCategoryId, CancellationToken cancellationToken)
        {
            try
            {
                var bookmark = await _bookmarkRepository.GetByIdAsync(bookmarkId, cancellationToken);

                if (!bookmark.Success || bookmark.Data == null)
                {
                    return new OperationResult
                    {
                        Success = false,
                        ErrorMessage = "Bookmark not found."
                    };
                }

                bookmark.Data.CategoryId = newCategoryId;
                await _bookmarkRepository.UpdateAsync(bookmark.Data, cancellationToken);

                return new OperationResult
                {
                    Success = true
                };
            }
            catch (Exception ex) when (ex is DbUpdateConcurrencyException or DbUpdateException)
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
