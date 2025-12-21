using System.ComponentModel.DataAnnotations;

namespace SmartBookmarkApi.DTOs
{
    public class BookmarkVisitCountDto
    {
        public int BookmarkId { get; set; }
        public string BookmarkTitle { get; set; } = "";
        public int VisitCount { get; set; }
    }
}
