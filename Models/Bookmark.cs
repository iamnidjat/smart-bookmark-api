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

        public ICollection<string> Categories = []; // new List<string>();

        public ICollection<string> Tags = [];

        public int UserId { get; set; }

        public User? User { get; set; }
    }
}
