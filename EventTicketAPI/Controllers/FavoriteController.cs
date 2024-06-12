using EventTicketAPI.Entities;
using EventTicketAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EventTicketAPI.Filter;
using EventTicketAPI.Dtos.Event;

namespace EventTicketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IEventService _eventService;
        public FavoriteController(IEventService eventService)
        {
            _eventService = eventService;
        }
        [Authorize]
        [RoleFilter("Member")]
        [HttpPost("favoriteevent/{EventId}")]
        public async Task<IActionResult> FavoriteEvent(int EventId)
        {
            var user = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Favorite favorite = new()
            {
                UserId = user,
                EventId = EventId
            };
            var addfavorite = await _eventService.AddFavoriteEvent(favorite, user);
            if (addfavorite == null)
            {
                return BadRequest("This event is already in your favorites");
            }
            return Ok("This event has been added to your favorites");
        }
        [Authorize]
        [RoleFilter("Member")]
        [HttpGet("viewmyfavorites")]
        public async Task<ActionResult<IEnumerable<EventReturnDto>>> ViewMyFavorites()
        {
            var userid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _eventService.ShowMyFavorites(userid);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }
        [Authorize]
        [RoleFilter("Member")]
        [HttpDelete("unfavoriteevent/{EventId}")]
        public async Task<IActionResult> RemoveFavorite(int EventId)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _eventService.UnfavoriteEvent(userId, EventId);
            return Ok();
        }
    }
}
