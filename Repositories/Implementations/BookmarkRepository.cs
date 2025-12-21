using Microsoft.EntityFrameworkCore;
using SmartBookmarkApi.Data;
using SmartBookmarkApi.Models;
using SmartBookmarkApi.Repositories.Interfaces;
using SmartBookmarkApi.Utilities;

namespace SmartBookmarkApi.Repositories.Implementations
{
    // BookmarkRepository contains data-access logic specific to the Bookmark entity.
    // It inherits all basic CRUD operations from the generic Repository<T>,
    // and can define additional bookmark-specific DB queries when needed.
    // Example: Get bookmarks by user, search bookmarks, filter by tags, etc.

    public class BookmarkRepository : Repository<Bookmark>, IBookmarkRepository
    {
        public BookmarkRepository(AppDbContext context, ILogger<BookmarkRepository> logger)
        : base(context, logger)
        {
        }

        public override async Task<OperationResult> UpdateAsync(int id, Bookmark updatedEntity)
        {
            var result = await base.UpdateAsync(id, updatedEntity);
            if (!result.Success) return result;
            try
            {

                var existing = await _context.Bookmarks.FindAsync(id);
                if (existing != null)
                {
                    existing.Tags = updatedEntity.Tags; // Tags is not updated by SetValues
                    await _context.SaveChangesAsync();
                }

                return new OperationResult { Success = true };
            }
            catch (Exception ex) when (ex is DbUpdateConcurrencyException or DbUpdateException or OperationCanceledException)
            {
                _logger.LogError(ex, "Failed to update Tags for Bookmark with ID {Id}", id);
                return new OperationResult { Success = false, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating Tags for Bookmark with ID {Id}", id);
                return new OperationResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<List<Bookmark>> FilterAsync(string filterWord)
        {
            return await _context.Bookmarks
                .AsNoTracking()
                .Where(b =>
                    b.Title.Contains(filterWord) ||
                    b.Url.Contains(filterWord) ||
                    (b.Content ?? "").Contains(filterWord) ||
                    (b.Description ?? "").Contains(filterWord))
                .ToListAsync();
        }
    }
}
