using System.ComponentModel.DataAnnotations;

namespace WeddingSite.Data.Models
{
    public class Guest
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = default!;

        // If you want to allow guests to bring additional people
        public int PartySize { get; set; } = 1;

        public bool IsAttending { get; set; } = false;

        public string? Notes { get; set; }
    }
}
