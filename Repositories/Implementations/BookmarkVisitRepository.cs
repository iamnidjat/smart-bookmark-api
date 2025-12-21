using Microsoft.EntityFrameworkCore;
using SmartBookmarkApi.Data;
using SmartBookmarkApi.DTOs;
using SmartBookmarkApi.Models;
using SmartBookmarkApi.Repositories.Interfaces;
using SmartBookmarkApi.Utilities;

namespace SmartBookmarkApi.Repositories.Implementations
{
    public class BookmarkVisitRepository : IBookmarkVisitRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BookmarkVisitRepository> _logger;
        public BookmarkVisitRepository(AppDbContext context, ILogger<BookmarkVisitRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task RegisterVisitAsync(int bookmarkId)
        {
            var bookmark = await _context.Bookmarks.FindAsync(bookmarkId);

            if (bookmark == null)
            {
                return;
            }

            bookmark.VisitCount++;

            _context.BookmarkVisits.Add(new BookmarkVisit
            {
                BookmarkId = bookmarkId
            });

            await _context.SaveChangesAsync();
        }

        public async Task<List<BookmarkVisitCountDto>> GetMostVisitedAsync(DateTime from, int take)
        {
            return await _context.BookmarkVisits
                .Where(b => b.VisitedAt >= from)
                .GroupBy(b => b.Bookmark)
                .Select(g => new BookmarkVisitCountDto
                {
                    BookmarkId = g.Key.Id,
                    BookmarkTitle = g.Key.Title,
                    VisitCount = g.Count()
                })
                .OrderByDescending(b => b.VisitCount)
                .Take(take)
                .ToListAsync();
        }
    }
}
