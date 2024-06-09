using EventTicketAPI.Dtos;
using EventTicketAPI.Services;
using Microsoft.AspNetCore.Mvc;
using EventTicketAPI.Filter;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace EventTicketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketTypeController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public TicketTypeController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [Authorize]
        [RoleFilter("Admin")]
        [HttpPost("addtickettype")]
        public async Task<IActionResult> AddTicketType(AddTicketTypeDto ticketType)
        {
            var forTicketsAvailable = await _ticketService.AddTicketTypeService(ticketType);
            if (Convert.ToInt32(forTicketsAvailable) == 0)
            {
                return BadRequest("Error: Capacity of an event Maybe has been filled OR there might be other problems");
            }
            
            return Ok($"ticket's capacity left: {forTicketsAvailable} seats");
        }
        [Authorize]
        [RoleFilter("Admin")]
        [HttpPut("changeeventtype/{ticketTypeId}")]
        public async Task<IActionResult> ChangeTicketType(int ticketTypeId, AddTicketTypeDto ticketType)
        {
            await _ticketService.UpdateTicketType(ticketTypeId, ticketType);
            
            return Ok();
        }

        [HttpGet("seetickettypes/{eventId}")]
        public async Task<ActionResult<IEnumerable<TicketTypeReturnDto>>> SeeTicketTypes(int eventId)
        {
            
            var tickettypes = await _ticketService.ShowTicketTypes(eventId);
            if (tickettypes == null)
            {
                return NotFound();
            }
            return Ok(tickettypes);
        }
        [Authorize]
        [RoleFilter("Admin")]
        [HttpDelete("removetickettype/{TicketTypeId}/{EventId}")]
        public async Task<IActionResult> RemoveTicketType(int TicketTypeId, int EventId)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _ticketService.RemoveTicketType(TicketTypeId, EventId, userId);
            return Ok();
        }
    }
}
