using SmartBookmarkApi.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace SmartBookmarkApi.Models
{
    public class BookmarkVisit : IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int BookmarkId { get; set; }
        [Required]
        public DateTime VisitedAt { get; set; } = DateTime.UtcNow;

        public Bookmark Bookmark { get; set; } = null!;

        // null-forgiving: tells compiler EF will set this property, avoids nullable warnings
        // default! - suppresses warning, tells compiler “trust me, this will be set later"
    }
}
