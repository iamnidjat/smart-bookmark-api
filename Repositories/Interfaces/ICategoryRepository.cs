using SmartBookmarkApi.Models;

namespace SmartBookmarkApi.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<List<Category>> FilterAsync(string filterWord);
    }
}
