using RazorClassLib.Data;
using RazorClassLib.Services;
using SQLite;

namespace TicketsAreUs.Services;

public class OccasionService : IOccasionService
{

    public string _dbPath;
    public string DatabaseName { get; set; } = "pec_tickets.db3";
    public string StatusMessage { get; set; }
    private SQLiteAsyncConnection conn;

    public OccasionService()
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
        await conn.CreateTableAsync<Occasion>();

    }

    public async Task<List<Occasion>> GetAllOccasions()
    {
        try
        {
            await Init();

            return await conn.Table<Occasion>().ToListAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return new List<Occasion>();
    }

    public async Task AddNewOccasion(Occasion occasion)
    {
        try
        {
            await Init();

            if (string.IsNullOrEmpty(occasion.OccasionName))
            {
                throw new Exception("Valid name required");
            }

            var result = await conn.InsertOrReplaceAsync(occasion);

            StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, occasion.OccasionName);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", occasion.OccasionName, ex.Message);   
        }
    }

    public async Task<Occasion> GetOccasionId(string name)
    {
        await Init();

        return await conn.Table<Occasion>()
            .Where(o => o.OccasionName == name)
            .FirstOrDefaultAsync();   
    }

    public async Task<Occasion> GetOccasion(int id)
    {
        await Init();

        return await conn.Table<Occasion>()
            .Where(o => o.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task DropTables()
    {
        await Init();

        await conn.DeleteAllAsync<Occasion>();
       // await conn.DropTableAsync<Occasion>();
    }
}
