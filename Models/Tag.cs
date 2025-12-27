using SmartBookmarkApi.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace SmartBookmarkApi.Models
{
    public class Tag : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        public ICollection<Bookmark> Bookmarks { get; set; } = [];
    }
}
