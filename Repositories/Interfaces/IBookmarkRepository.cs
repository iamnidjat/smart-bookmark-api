using SmartBookmarkApi.Models;

namespace SmartBookmarkApi.Repositories.Interfaces
{
    public interface IBookmarkRepository : IRepository<Bookmark>
    {
        Task<List<Bookmark>> FilterAsync(int userId, string filterWord, CancellationToken cancellationToken);
    }
}
