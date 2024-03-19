using Microsoft.EntityFrameworkCore;
using RazorClassLib.Data;
using RazorClassLib.Request;
using RazorClassLib.Services;
using Telemetry;
using WebApp.Exceptions;

namespace WebApp.Services;

public partial class TicketService : ITicketService
{
    private readonly ILogger<TicketService> logger;
    private IDbContextFactory<TicketContext> contextFactory;

    [LoggerMessage(Level = LogLevel.Warning, Message = "Added Tickets To Database")]
    static partial void LogAddTicket(ILogger logger, string description);

    [LoggerMessage(Level = LogLevel.Information, Message = "Updating Tickets In The Database")]
    static partial void LogUpdateTicket(ILogger logger, string description);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Getting Tickets From The Database")]
    static partial void LogGetTicket(ILogger logger, string description);

    [LoggerMessage(Level = LogLevel.Information, Message = "Getting All Tickets In The Database")]
    static partial void LogGetAllTickets(ILogger logger, string description);

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
        LogAddTicket(logger, $"Added {ticket.Guid} To The Database");
    }

    public Task DropTables()
    {
        throw new NotImplementedException();
    }

    public async Task<List<Ticket>> GetAllTickets()
    {
        using var myActivity = ParkerTraces.TicketSource.StartActivity("Getting All Tickets");
        LogGetAllTickets(logger, $"Getting All The Tickets");

        var context = contextFactory.CreateDbContext();
        return await context.Tickets
            .Include(t => t.Occasion)
            .ToListAsync();
    }

    public async Task<Ticket> GetTicket(int id)
    {
        var context = contextFactory.CreateDbContext();
        var result = await context.Tickets
            .Where(t => t.Id == id)
            .Include(t => t.Occasion)
            .FirstOrDefaultAsync();

        LogGetTicket(logger, $"Getting {result!.Id} From The Database");

        if (result is not null)
        {
            return result;
        }

        throw new Exception();
    }

    public async Task<Ticket> GetTicketId(Guid guid)
    {
        var context = contextFactory.CreateDbContext();
        var result = await context.Tickets
            .Where(t => t.Guid == guid)
            .Include(t => t.Occasion)
            .FirstOrDefaultAsync();

        if (result is not null)
        {
            LogGetTicket(logger, $"Getting {result!.Guid} From The Database");
            return result;
        }

        throw new Exception();
    }

    public async Task UpdateTicket(int id)
    {
        var context = await contextFactory.CreateDbContextAsync();
        var oldTicket = await context.Tickets.Where(t => t.Id == id).FirstOrDefaultAsync();

        if (oldTicket == null)
        {
            throw new Exception();
        }

        if (oldTicket.IsUsed == false)
        {
            oldTicket.IsUsed = true;
            context.Update(oldTicket);
            await context.SaveChangesAsync();
            LogUpdateTicket(logger, $"Updated {oldTicket.Guid} To New Values");
        }
        else
        {
            throw new TicketAlreadyScannedException();
        }
    }
}
