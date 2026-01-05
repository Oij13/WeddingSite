using Microsoft.EntityFrameworkCore;
using WeddingSite.Data;
using WeddingSite.Data.Models;

namespace WeddingSite.Services
{
    public interface IRegistryService
    {
        Task<List<RegistryItem>> GetRegistry();
        Task AddItem(RegistryItem item);
    }
    public class RegistryService(WeddingDbContext db, ILogger<RegistryService> logger) : IRegistryService
    {
        private readonly WeddingDbContext _db = db;
        private readonly ILogger<RegistryService> _logger = logger;

        public async Task<List<RegistryItem>> GetRegistry()
        {
            try
            {
                return await _db.RegistryItems.OrderByDescending(r => r.Id).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Failed to get items from registry.");
                return [];
            }
        }

        public async Task AddItem(RegistryItem item)
        {
            try
            {
                await _db.RegistryItems.AddAsync(item);
            }
            catch(Exception e)
            {
                _logger.LogError(e,"Failed to add item to registry.");
            }
        }
    }
}