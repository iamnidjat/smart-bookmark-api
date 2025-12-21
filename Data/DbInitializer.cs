using Microsoft.EntityFrameworkCore;
using SmartBookmarkApi.Models;

namespace SmartBookmarkApi.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(AppDbContext context)
        {
            // Seed Users
            if (!await context.Users.AnyAsync())
            {
                var users = new List<User>
                {
                    new User { Username = "alice", Email = "alice@example.com", Password = BCrypt.Net.BCrypt.HashPassword("password123") },
                    new User { Username = "bob", Email = "bob@example.com", Password = BCrypt.Net.BCrypt.HashPassword("password123") }
                };

                context.Users.AddRange(users);
                await context.SaveChangesAsync();
            }

            // Seed Categories
            if (!await context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Tech", Description = "Technology related bookmarks" },
                    new Category { Name = "News", Description = "News and media" }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // Seed Bookmarks
            if (!await context.Bookmarks.AnyAsync())
            {
                var alice = await context.Users.FirstAsync(u => u.Username == "alice");
                var bob = await context.Users.FirstAsync(u => u.Username == "bob");
                var tech = await context.Categories.FirstAsync(c => c.Name == "Tech");
                var news = await context.Categories.FirstAsync(c => c.Name == "News");

                var bookmarks = new List<Bookmark>
                {
                    new Bookmark
                    {
                        Title = "ASP.NET Core Docs",
                        Url = "https://docs.microsoft.com/aspnet/core",
                        Description = "Official ASP.NET Core documentation",
                        Tags = new List<string>(), // empty
                        UserId = alice.Id,
                        CategoryId = tech.Id,
                        VisitCount = 10
                    },
                    new Bookmark
                    {
                        Title = "BBC News",
                        Url = "https://www.bbc.com/news",
                        Description = "Latest world news",
                        Tags = new List<string>(), // empty
                        UserId = bob.Id,
                        CategoryId = news.Id,
                        VisitCount = 5
                    }
                };

                context.Bookmarks.AddRange(bookmarks);
                await context.SaveChangesAsync();
            }

            // Seed BookmarkVisits
            if (!await context.BookmarkVisits.AnyAsync())
            {
                var bookmark = await context.Bookmarks.FirstAsync();
                var visits = new List<BookmarkVisit>
                {
                    new BookmarkVisit { BookmarkId = bookmark.Id, VisitedAt = DateTime.UtcNow.AddDays(-2) },
                    new BookmarkVisit { BookmarkId = bookmark.Id, VisitedAt = DateTime.UtcNow.AddDays(-1) }
                };

                context.BookmarkVisits.AddRange(visits);
                await context.SaveChangesAsync();
            }

            // Seed RefreshTokens
            if (!await context.RefreshTokens.AnyAsync())
            {
                var alice = await context.Users.FirstAsync(u => u.Username == "alice");
                var token = new RefreshToken
                {
                    Token = Guid.NewGuid().ToString(),
                    UserId = alice.Id,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    IsUsed = false
                };

                context.RefreshTokens.Add(token);
                await context.SaveChangesAsync();
            }
        }
    }
}
