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
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BookmarkVisit> BookmarkVisits { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // for safety

            modelBuilder.Entity<BookmarkVisit>()
                .HasOne(v => v.Bookmark)
                .WithMany(b => b.Visits)
                .HasForeignKey(v => v.BookmarkId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RefreshToken>()
               .HasOne(v => v.User)
               .WithMany(b => b.RefreshTokens)
               .HasForeignKey(v => v.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bookmark>()
                .HasOne(v => v.User)
                .WithMany(b => b.Bookmarks)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasOne(v => v.User)
                .WithMany(b => b.Categories)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Restrict); 
            // Categories → User: **cannot cascade** due to SQL Server multiple cascade path restriction

            modelBuilder.Entity<Bookmark>()
               .HasOne(v => v.Category)
               .WithMany(b => b.Bookmarks)
               .HasForeignKey(v => v.CategoryId)
               .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Bookmark>()
                .HasMany(b => b.Tags)
                .WithMany(t => t.Bookmarks);
        }
    }
}
