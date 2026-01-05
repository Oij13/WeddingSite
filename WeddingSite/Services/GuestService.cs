using Microsoft.EntityFrameworkCore;
using WeddingSite.Data;
using WeddingSite.Data.Models;

namespace WeddingSite.Services
{
    public interface IGuestService
    {
        Task<List<Guest>> GetGuests();
        Task AddGuest(Guest guest);
    } 
    public class GuestService(WeddingDbContext db, ILogger<GuestService> logger) : IGuestService
    {
        private readonly WeddingDbContext _db = db;
        private readonly ILogger<GuestService> _logger = logger;

        public async Task<List<Guest>> GetGuests()
        {
            try
            {
                return await _db.Guests.OrderByDescending(g => g.Id).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Failed to get guests");
                return [];
            }
        }

        public async Task AddGuest(Guest guest)
        {
            try
            {
                await _db.Guests.AddAsync(guest);
            }
            catch(Exception e)
            {
                _logger.LogError(e,"Failed to add guest.");
            }
        }
    }
}