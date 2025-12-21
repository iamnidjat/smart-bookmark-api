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

        public Bookmark? Bookmark { get; set; } 
        // default! - suppresses warning, tells compiler “trust me, this will be set later"
    }
}
