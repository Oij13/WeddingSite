using System.ComponentModel.DataAnnotations;

namespace WeddingSite.Data.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }
         
        // Recommended: save file under wwwroot/images and store relative Path
        [Required, MaxLength(500)]
        public string Path { get; set; } = default!; // e.g. "images/ceremony.jpg"

        [MaxLength(200)]
        public string? FileName { get; set; }

        [MaxLength(100)]
        public string? ContentType { get; set; }
    }
}
