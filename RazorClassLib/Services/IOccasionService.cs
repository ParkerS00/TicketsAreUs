using RazorClassLib.Data;

namespace RazorClassLib.Services;
public interface IOccasionService
{
    public Task<List<Occasion>> GetAllOccasions();
    public Task<Occasion> GetOccasionId(string name);
    public Task<Occasion> GetOccasion(int id);
    public Task AddNewOccasion(Occasion occasion);
    public Task DropTables();
}
