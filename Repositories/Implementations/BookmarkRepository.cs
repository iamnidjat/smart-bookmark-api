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

        public override async Task<OperationResult> UpdateAsync(int id, Bookmark updatedEntity, CancellationToken cancellationToken)
        {
            var result = await base.UpdateAsync(id, updatedEntity, cancellationToken);
            if (!result.Success) return result;
            try
            {
                // Including Tags so EF can track the collection
                var existing = await _context.Bookmarks
                    .Include(b => b.Tags)
                    .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

                if (existing != null)
                {
                    // Clearing old tags
                    existing.Tags.Clear();

                    foreach (var tag in updatedEntity.Tags)
                    {
                        // Reusing existing tag if it exists in DB, otherwise adding new
                        var existingTag = await _context.Tags
                            .FirstOrDefaultAsync(t => t.Name == tag.Name, cancellationToken);

                        existing.Tags.Add(existingTag ?? tag);
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                }

                return new OperationResult { Success = true };
            }
            catch (Exception ex) when (ex is DbUpdateConcurrencyException or DbUpdateException)
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

        public async Task<List<Bookmark>> FilterAsync(int userId, string filterWord, CancellationToken cancellationToken)
        {
            return await _context.Bookmarks
                .AsNoTracking()
                .Where(b =>
                    b.UserId == userId &&
                    b.Title.Contains(filterWord) ||
                    b.Url.Contains(filterWord) ||
                    (b.Content ?? "").Contains(filterWord) ||
                    (b.Description ?? "").Contains(filterWord))
                .ToListAsync(cancellationToken);
        }
    }
}
