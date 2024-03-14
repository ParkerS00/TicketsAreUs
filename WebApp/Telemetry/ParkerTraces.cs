using System.Diagnostics;

namespace Telemetry;

public static class ParkerTraces
{
    public static readonly string GetAllOccasionsName = "Get All Occasions Trace";
    public static readonly string GetAllTicketsName = "Get All Ticket Trace";

    public static readonly ActivitySource OccasionSource = new(GetAllOccasionsName);
    public static readonly ActivitySource TicketSource = new(GetAllTicketsName);
}
