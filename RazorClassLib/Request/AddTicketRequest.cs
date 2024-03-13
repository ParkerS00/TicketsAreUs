using RazorClassLib.Data;

namespace RazorClassLib.Request;

public class AddTicketRequest
{
    public int Id { get; set; }
    public int? OccasionId { get; set; }
    public Guid? Guid { get; set; }
    public bool? IsUsed { get; set; }
}
