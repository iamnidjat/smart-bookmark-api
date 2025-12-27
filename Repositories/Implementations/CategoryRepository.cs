using Microsoft.EntityFrameworkCore;
using SmartBookmarkApi.Data;
using SmartBookmarkApi.Models;
using SmartBookmarkApi.Repositories.Interfaces;

namespace SmartBookmarkApi.Repositories.Implementations
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context, ILogger<CategoryRepository> logger)
        : base(context, logger)
        {
        }

        public async Task<List<Category>> FilterAsync(string filterWord, CancellationToken cancellationToken)
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(b => b.Name.Contains(filterWord) || (b.Description ?? "").Contains(filterWord))
                .ToListAsync(cancellationToken);
        }
    }
}
