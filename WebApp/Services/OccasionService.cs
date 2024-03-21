using Microsoft.EntityFrameworkCore;
using RazorClassLib.Data;
using RazorClassLib.Services;
using Telemetry;

namespace WebApp.Services
{
    public partial class OccasionService : IOccasionService
    {
        private readonly ILogger<OccasionService> logger;
        private IDbContextFactory<TicketContext> contextFactory;

        [LoggerMessage(Level = LogLevel.Information, Message = "Added Occasions To Database")]
        static partial void LogAddOccasion(ILogger logger, string description);

        [LoggerMessage(Level = LogLevel.Information, Message = "Getting All Occasions From The Database")]
        static partial void LogGetAllOccasion(ILogger logger, string description);

        [LoggerMessage(Level = LogLevel.Information, Message = "Getting Occasion From The Database")]
        static partial void LogGetOccasion(ILogger logger, string description);

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
            LogAddOccasion(logger, $"Added {occasion.OccasionName} to the database");
            ParkerMetrics.occasionUpDown.Add(1);
            ParkerMetrics.occasionsChecked -= 1;
            ParkerMetrics.occasionsAdded += 1;
        }

        public Task DropTables()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Occasion>> GetAllOccasions()
        {
            using var myActivity = ParkerTraces.OccasionSource.StartActivity("Getting All Occasions");
            ParkerMetrics.occasionCounter.Add(3);
            ParkerMetrics.occasionsChecked += 3;
            LogGetAllOccasion(logger, $"Getting All Occasions");

            var context = contextFactory.CreateDbContext();
            return await context.Occasions
                .Include(o => o.Tickets)
                .ToListAsync();
        }

        public async Task<Occasion> GetOccasion(int id)
        {
            var context = contextFactory.CreateDbContext();
            var result = await context.Occasions
                .Where(o => o.Id == id)
                .Include(o => o.Tickets)
                .FirstOrDefaultAsync();

            LogGetOccasion(logger, $"Getting {result!.OccasionName} From The Database");
            ParkerMetrics.occasionUpDown.Add(-1);
            ParkerMetrics.occasionsChecked += 1;

            if (result is not null)
            {
                return result;
            }

            throw new Exception();
        }

        public async Task<Occasion> GetOccasionId(string name)
        {
            var context = contextFactory.CreateDbContext();
            var result = await context.Occasions
                .Where(o => o.OccasionName == name)
                .Include(o => o.Tickets)
                .FirstOrDefaultAsync();

            if (result is not null)
            {
                return result;
            }

            throw new Exception();
        }
    }
}
