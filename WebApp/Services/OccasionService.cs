﻿using Microsoft.EntityFrameworkCore;
using RazorClassLib.Data;
using RazorClassLib.Services;
using Telemetry;

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
            using var myActivity = ParkerTraces.OccasionSource.StartActivity("Getting All Occasions");
            ParkerMetrics.occasionCounter.Add(3);

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
