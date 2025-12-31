using SmartBookmarkApi.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace SmartBookmarkApi.Models
{
    public class Bookmark : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = "";

        [Required]
        public string Url { get; set; } = "";

        public string? Description { get; set; }

        public string? Content { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int VisitCount { get; set; } // all-time
        
        // EF Core will create third table for many-to-many relationhsip automatically
        public ICollection<Tag> Tags { get; set; } = []; // new List<Tag>();
        public ICollection<BookmarkVisit> Visits { get; set; } = [];

        [Required]
        public int UserId { get; set; } // bookmark must belong to user

        public User User { get; set; } = null!;

        public int? CategoryId { get; set; } // bookmark can exist without category

        public Category? Category { get; set; }
    }
}
