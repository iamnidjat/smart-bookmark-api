using Microsoft.EntityFrameworkCore;
using SmartBookmarkApi.Models;

namespace SmartBookmarkApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Bookmark> Bookmarks { get; set; }
    }
}
