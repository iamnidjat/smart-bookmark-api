using SmartBookmarkApi.Models;

namespace SmartBookmarkApi.Repositories.Interfaces
{
    public interface IBookmarkRepository : IRepository<Bookmark>
    {
        Task<List<Bookmark>> FilterAsync(string filterWord, CancellationToken cancellationToken);
    }
}
