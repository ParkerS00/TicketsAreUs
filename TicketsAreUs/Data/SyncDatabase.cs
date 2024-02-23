using RazorClassLib.Services;
using RazorClassLib.Data;
using System.Net.Http.Json;

namespace TicketsAreUs.Data;

public class SyncDatabase
{
    private readonly ITicketService ticketService;
    private readonly IOccasionService ocasionService;
    private readonly HttpClient httpClient;
    public SyncDatabase(IOccasionService occasionService, ITicketService ticketService, HttpClient httpClient)
    {
        this.httpClient = httpClient;
        this.ticketService = ticketService;
        this.ocasionService = occasionService;
    }

    private string api { get; set; } = "";
    private bool isOnline { get; set; } = true;

    public async Task SyncAll(List<Ticket> OnlineTickets, List<Occasion> OnlineOccasions, int refreshRate)
    {
        GetPreferences();
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(refreshRate));
        while (Preferences.Default.Get("NetworkStatus", false) == true && await timer.WaitForNextTickAsync())
        {
            await SyncTickets(OnlineTickets);
            await SyncEvents(OnlineOccasions);
        }
    }

    public async Task SyncTickets(List<Ticket> OnlineTickets)
    {
        GetPreferences();
        var MauiTickets = await ticketService.GetAllTickets();
        OnlineTickets = await httpClient.GetFromJsonAsync<List<Ticket>>($"{api}/Ticket");
        foreach (var ticket in OnlineTickets)
        {
            var offlineTicket = MauiTickets
                .Where(t => t.Guid == ticket.Guid)
                .FirstOrDefault();

            if (offlineTicket is null)
            {
                await ticketService.AddNewTicket(ticket);
                offlineTicket = ticket;
                return;
            }

            if (offlineTicket.IsUsed == true)
            {
                if (ticket.IsUsed == false)
                {
                    await httpClient.PatchAsJsonAsync<Ticket>($"{api}/ticket/update/{ticket}", ticket);
                }
                else
                {
                    continue;
                }
            }
        }
    }

    public async Task SyncEvents(List<Occasion> OnlineOccasions)
    {
        GetPreferences();
        var MauiOccasions = await ocasionService.GetAllOccasions();
        foreach (var  occasion in OnlineOccasions)
        {
            var offlineOccasion = MauiOccasions
                .Where(o => o.OccasionName == occasion.OccasionName)
                .FirstOrDefault(); 

            if (offlineOccasion is null)
            {
                await ocasionService.AddNewOccasion(occasion);
                return;
            }
        }
    }

    public void GetPreferences()
    {
        api = Preferences.Default.Get("API", "");
        if (api == "https://localhost:7257" || api == "")
        {
            api = Preferences.Default.Get("API", "https://localhost:7257");
            Preferences.Default.Set("NetworkStatus", isOnline);
        }
        else
        {
            api = Preferences.Default.Get("API", "https://ticketsareus.azurewebsites.net");
            Preferences.Default.Set("NetworkStatus", isOnline);
        }
    }
}
