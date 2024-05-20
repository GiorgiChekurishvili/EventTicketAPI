using EventTicketAPI.Dtos;
using EventTicketAPI.Entities;
using EventTicketAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EventTicketAPI.Filter;


namespace EventTicketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketservice;
        public TicketController(ITicketService ticketService)
        {
            _ticketservice = ticketService;
        }
        [Authorize]
        [RoleFilter("Member")]
        [HttpPost("buyticket/{Eventid}/{TicketTypeId}/{TicketQuantity}")]
        public async Task<IActionResult> BuyTicket(int Eventid, int TicketTypeId, int TicketQuantity)
        {
            var user = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            BuyTicketDto buyTicket = new()
            {
                EventId = Eventid,
                TicketTypeId = TicketTypeId,
                TicketQuantity = TicketQuantity,
                UserId = user
                
            };
            
            var boughtticket = await _ticketservice.BuyTicketService(buyTicket);
            if (Convert.ToInt32(boughtticket) == 0)
            {
                return BadRequest("All the tickets in this section has already been sold");
            }
            
            return Ok("total price is: " + boughtticket + " lari");
        }
        [Authorize]
        [RoleFilter("Member")]
        [HttpDelete("refundticket/{ticketId}")]
        public async Task<IActionResult> RefundTicket(int ticketId)
        {
            var user = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _ticketservice.RefundTicketService(ticketId, user);
            return Ok();
        }
        [Authorize]
        [RoleFilter("Member")]
        [HttpGet("viewmytickets")]
        public async Task<ActionResult<MyTicketReturnDto>> ViewMyTickets()
        {
            var user = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _ticketservice.ShowTickets(user);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
