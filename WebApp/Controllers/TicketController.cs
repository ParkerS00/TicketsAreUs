using Microsoft.AspNetCore.Mvc;
using RazorClassLib.Data;
using RazorClassLib.Request;
using RazorClassLib.Services;
using WebApp.Exceptions;

namespace WebApp.Controllers;

[ApiController]
[Route("/[controller]")]
public class TicketController : ControllerBase
{
    private readonly ITicketService ticketService;

    public TicketController(ITicketService ticketService)
    {
        this.ticketService = ticketService;
    }

    [HttpGet()]
    public async Task<List<Ticket>> GetAll()
    {
        return await ticketService.GetAllTickets();
    }

    [HttpGet("{id}")]
    public async Task<Ticket> Get(int id)
    {
        return await ticketService.GetTicket(id);
    }

    //[HttpGet("{guid}")]
    //public async Task<Ticket> GetId(Guid guid)
    //{
    //    return await ticketService.GetTicketId(guid);
    //}

    [HttpPost("/add/{newRequest}")]
    public async Task<Ticket> Post([FromBody] AddTicketRequest newRequest)
    {
        var ticket = new Ticket
        {
            OccasionId = newRequest.OccasionId,
            Guid = newRequest.Guid,
            IsUsed = newRequest.IsUsed
        };

        await ticketService.AddNewTicket(ticket);
        return ticket;
    }

    [HttpPatch("/update/{ticket}")]
    public async Task Update([FromBody] Ticket ticket)
    {
        try
        {
            await ticketService.UpdateTicket(ticket.Id);
        }
        catch (TicketAlreadyScannedException)
        {
            throw new TicketAlreadyScannedException();
        }
    }
}
