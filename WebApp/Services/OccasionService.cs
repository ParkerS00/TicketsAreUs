using Microsoft.EntityFrameworkCore;
using RazorClassLib.Data;
using RazorClassLib.Services;

namespace WebApp.Services
{
    public class OccasionService : IOccasionService
    {
        private readonly ILogger<OccasionService> logger;
        private IDbContextFactory<TicketContext> contextFactory;

        public OccasionService(ILogger<OccasionService> logger, IDbContextFactory<TicketContext> contextFactory)
        {
            this.logger = logger;
            this.contextFactory = contextFactory;
        }
        public async Task AddNewOccasion(Occasion occasion)
        {
            var context = contextFactory.CreateDbContext();
            context.Add(occasion);
            await context.SaveChangesAsync();
        }

        public Task DropTables()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Occasion>> GetAllOccasions()
        {
            var context = contextFactory.CreateDbContext();
            return await context.Occasions
                .Include(o => o.Tickets)
                .ToListAsync();   
        }

        public async Task<Occasion> GetOccasion(int id)
        {
            var context = contextFactory.CreateDbContext();
            return await context.Occasions
                .Where(o => o.Id == id)
                .Include(o => o.Tickets)
                .FirstOrDefaultAsync();
        }

        public async Task<Occasion> GetOccasionId(string name)
        {
            var context = contextFactory.CreateDbContext();
            return await context.Occasions
                .Where(o => o.OccasionName == name)
                .Include(o => o.Tickets)
                .FirstOrDefaultAsync();
        }
    }
}
