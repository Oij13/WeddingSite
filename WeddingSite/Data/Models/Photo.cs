using System.ComponentModel.DataAnnotations;

namespace WeddingSite.Data.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }
        
        // Cloudinary public URL
        [Required, MaxLength(500)]
        public string Url { get; set; } = default!;

        // Cloudinary public ID (used for deleting/updating)
        [Required, MaxLength(200)]
        public string PublicId { get; set; } = default!;

        [MaxLength(200)]
        public string? FileName { get; set; }

        [MaxLength(100)]
        public string? ContentType { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
