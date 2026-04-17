using Microsoft.EntityFrameworkCore;
using WeddingSite.Data;
using WeddingSite.Data.Models;

namespace WeddingSite.Services
{
    public interface IGuestService
    {
        Task<List<Guest>> GetGuests();
        Task AddGuest(Guest guest);
        Task<Guest?> GetGuestByInvitationCode(Guid invitationCode);
        Task UpdateGuest(Guest guest);
        Task DeleteGuest(int id);
    } 
    public class GuestService(IDbContextFactory<WeddingDbContext> factory, ILogger<GuestService> logger) : IGuestService
    {
        private readonly IDbContextFactory<WeddingDbContext> _factory = factory;
        private readonly ILogger<GuestService> _logger = logger;

        public async Task<List<Guest>> GetGuests()
        {
            try
            {
                using var db = await _factory.CreateDbContextAsync();
                return await db.Guests.OrderByDescending(g => g.Id).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get guests");
                return [];
            }
        }

        public async Task AddGuest(Guest guest)
        {
            try
            {
                using var db = await _factory.CreateDbContextAsync();
                await db.Guests.AddAsync(guest);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to add guest.");
            }
        }

        public async Task<Guest?> GetGuestByInvitationCode(Guid invitationCode)
        {
            try
            {
                using var db = await _factory.CreateDbContextAsync();
                return await db.Guests.FirstOrDefaultAsync(g => g.InvitationCode == invitationCode);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get guest by invitation code.");
                return null;
            }
        }

        public async Task UpdateGuest(Guest guest)
        {
            try
            {
                using var db = await _factory.CreateDbContextAsync();
                db.Guests.Update(guest);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to update guest.");
                throw;
            }
        }

        public async Task DeleteGuest(int id)
        {
            try
            {
                using var db = await _factory.CreateDbContextAsync();
                var guest = await db.Guests.FindAsync(id);
                if (guest is null) return;
                db.Guests.Remove(guest);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to delete guest.");
                throw;
            }
        }
    }
}