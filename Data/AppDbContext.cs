using Microsoft.EntityFrameworkCore;
using SmartBookmarkApi.Models;

namespace SmartBookmarkApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        //    Database.EnsureCreated(); // development-only approach
        }

        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<BookmarkVisit> BookmarkVisits { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
