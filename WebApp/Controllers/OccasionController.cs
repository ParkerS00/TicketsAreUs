using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using RazorClassLib.Data;
using RazorClassLib.Request;
using RazorClassLib.Services;

namespace WebApp.Controllers;

[ApiController]
[Route("/[controller]")]
public class OccasionController : ControllerBase
{
    private readonly IOccasionService occasionService;
    public OccasionController (IOccasionService occasionService)
    {
        this.occasionService = occasionService;
    }

    [HttpGet()]
    public async Task<List<Occasion>> GetAll()
    {
        return await occasionService.GetAllOccasions();
    }

    [HttpGet("{id}")]
    public async Task<Occasion> Get(int id)
    {
        return await occasionService.GetOccasion(id);
    }

    [HttpPost("{request}")]
    public async Task<Occasion> Post([FromBody] AddOccasionRequest request)
    {
        var occasion = new Occasion
        {
            OccasionName = request.OccasionName,
            TimeOfDay = request.TimeOfDay,
            Tickets = request.Tickets
        };

        await occasionService.AddNewOccasion(occasion);
        return occasion;
    }

    //[HttpGet("{name}")]
    //public async Task<Occasion> GetId(string name)
    //{
    //    return await occasionService.GetOccasionId(name);
    //}
}
