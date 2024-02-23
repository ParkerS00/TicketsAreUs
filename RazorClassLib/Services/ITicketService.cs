using RazorClassLib.Data;
using RazorClassLib.Request;

namespace RazorClassLib.Services;

public interface ITicketService
{
    public Task<Ticket> GetTicketId(Guid guid);
    public Task<Ticket> GetTicket(int id);
    public Task<List<Ticket>> GetAllTickets();
    public Task AddNewTicket(Ticket ticket);
    public Task UpdateTicket(int id);
    public Task DropTables();
}
