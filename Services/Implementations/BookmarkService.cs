using SmartBookmarkApi.Models;
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

        public BookmarkService(IBookmarkRepository bookmarkRepository)
        {
            _bookmarkRepository = bookmarkRepository;
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
    }
}
