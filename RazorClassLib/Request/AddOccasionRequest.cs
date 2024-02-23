using RazorClassLib.Data;
using SQLite;

namespace RazorClassLib.Request;

public class AddOccasionRequest
{
    public int Id { get; set; }

    public string? OccasionName { get; set; }

    public DateTime? TimeOfDay { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
