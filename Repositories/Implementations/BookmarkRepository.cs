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
    }
}
