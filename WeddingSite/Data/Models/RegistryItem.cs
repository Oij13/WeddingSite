using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingSite.Data.Models
{
    public class RegistryItem
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = default!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Price { get; set; }

        [MaxLength(500)]
        public string? Url { get; set; }

        // Optional relationship to a Photo (store path in Photo.Path)
        public int? PhotoId { get; set; }
        public Photo? Photo { get; set; }
    }
}
