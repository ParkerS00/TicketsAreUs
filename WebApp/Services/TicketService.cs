using Microsoft.EntityFrameworkCore;
using RazorClassLib.Data;
using RazorClassLib.Request;
using RazorClassLib.Services;
using WebApp.Exceptions;

namespace WebApp.Services;

public class TicketService : ITicketService
{
    private readonly ILogger<TicketService> logger;
    private IDbContextFactory<TicketContext> contextFactory;
    public TicketService(ILogger<TicketService> logger, IDbContextFactory<TicketContext> contextFactory)
    {
        this.logger = logger;
        this.contextFactory = contextFactory;
    }

    public async Task AddNewTicket(Ticket ticket)
    {
        var context = contextFactory.CreateDbContext();
        context.Add(ticket);
        await context.SaveChangesAsync();
    }

    public Task DropTables()
    {
        throw new NotImplementedException();
    }

    public async Task<List<Ticket>> GetAllTickets()
    {
        var context = contextFactory.CreateDbContext();
        return await context.Tickets
            .Include(t => t.Occasion)
            .ToListAsync();
    }

    public async Task<Ticket> GetTicket(int id)
    {
        var context = contextFactory.CreateDbContext();
        return await context.Tickets
            .Where(t => t.Id == id)
            .Include(t => t.Occasion)
            .FirstOrDefaultAsync();
    }

    public async Task<Ticket> GetTicketId(Guid guid)
    {
        var context = contextFactory.CreateDbContext();
        return await context.Tickets
            .Where(t => t.Guid == guid)
            .Include(t => t.Occasion)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateTicket(int id)
    {
        var context = await contextFactory.CreateDbContextAsync();
        var oldTicket = await context.Tickets.Where(t => t.Id == id).FirstOrDefaultAsync();
        if (oldTicket == null) { throw new Exception(); }
        if (oldTicket.IsUsed == false)
        {
            oldTicket.IsUsed = true;
            context.Update(oldTicket);
            await context.SaveChangesAsync();
        }
        else
        {
            throw new TicketAlreadyScannedException();
        }
    }
}
