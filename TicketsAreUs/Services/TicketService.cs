using RazorClassLib.Data;
using RazorClassLib.Services;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

namespace TicketsAreUs.Services;

public class TicketService : ITicketService
{
    public string _dbPath;
    public string DatabaseName { get; set; } = "pec_tickets.db3";
    public string StatusMessage { get; set; }
    private SQLiteAsyncConnection conn;

    public TicketService()
    {
        try
        {
            _dbPath = Path.Combine(FileSystem.Current.AppDataDirectory, DatabaseName);
        }
        catch
        {
            _dbPath = "";
        }
    }

    private async Task Init()
    {
        if (conn != null) { return; }
        conn = new SQLiteAsyncConnection(_dbPath);
        await conn.CreateTableAsync<Ticket>();

    }

    public async Task<Ticket> GetTicketId(Guid guid)
    {
        await Init();

        return await conn.Table<Ticket>()
            .Where(t => t.Guid == guid)
            .FirstOrDefaultAsync();
    }

    public async Task<Ticket> GetTicket(int id)
    {
        await Init();

        return await conn.Table<Ticket>()
            .Where(t => t.Id == id)
            .FirstOrDefaultAsync();  
    }

    public async Task<List<Ticket>> GetAllTickets()
    {
        try
        {
            await Init();

            return await conn.GetAllWithChildrenAsync<Ticket>();    
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return new List<Ticket>();
    }

    public async Task AddNewTicket(Ticket ticket)
    {
        try
        {
            await Init();

            var result = await conn.InsertOrReplaceAsync(ticket);

            StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, ticket.Guid);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", ticket.Guid, ex.Message);
        }
    }

    public async Task UpdateTicket(int id)
    {
        try
        {
            await Init();

            var oldTicket = await conn.Table<Ticket>()
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            oldTicket.IsUsed = true;
            await conn.UpdateAsync(oldTicket);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", id, ex.Message);
        }
    }

    public async Task DropTables()
    {
        await Init();

        await conn.DeleteAllAsync<Ticket>();
       // await conn.DropTableAsync<Ticket>();
    }
}
