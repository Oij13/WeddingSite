using System.ComponentModel.DataAnnotations;

namespace WeddingSite.Data.Models;

public class Guest
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string InvitationCode { get; set; } = string.Empty;

    // How many people can this guest bring (including themselves)
    public int MaxPartySize { get; set; } = 1;

    // RSVP Response
    public bool HasRsvped { get; set; }
    public bool IsAttending { get; set; }
    public int PartySize { get; set; } = 1; // How many they're actually bringing

    // Optional details
    [MaxLength(1000)]
    public string? DietaryRestrictions { get; set; }

    public string? Notes { get; set; }

    public DateTime? RsvpDate { get; set; }
}