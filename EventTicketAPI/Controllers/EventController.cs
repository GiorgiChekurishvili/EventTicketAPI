using EventTicketAPI.Dtos;
using EventTicketAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventTicketAPI.Filter;
using EventTicketAPI.Entities;
namespace EventTicketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        
        [HttpGet("eventcategories")]
        public async Task<ActionResult<IEnumerable<CategoryReturnDto>>> AllCategories()
        {
            var categories = await _eventService.ShowCategories();
            if (!categories.Any())
            {
                return NotFound();
            }
            return Ok(categories);
        }

        [Authorize]
        [RoleFilter("Admin")]
        [HttpPost("publishevent")]
        public async Task<IActionResult> PublishEvent(AddEventDto addEvent)
        {

            var currentdate = DateTime.Now;
            if (addEvent.EventDate <= currentdate)
            {
                return BadRequest("Datetime is incorrect");
            }
            var _event = await _eventService.AddEventService(addEvent);
            if (_event == null)
            {
                return BadRequest("Event's name already exists");
            }
            if (addEvent.Capacity == 0)
            {
                return BadRequest("capacity can't be zero");
            }
            return Ok("Event has been added");
        }

        [Authorize]
        [RoleFilter("Admin")]
        [HttpPut("updateevent/{id}")]
        public async Task<IActionResult> UpdateEvent(int id, AddEventDto addEvent)
        {
            var currentdate = DateTime.Now;
            if (addEvent.EventDate > currentdate)
            {
                return BadRequest("Datetime is incorrect");
            }
            if (addEvent.Capacity == 0)
            {
                return BadRequest("capacity can't be zero");
            }
            await _eventService.UpdateEvent(id, addEvent);
            return Ok();
        }

        [Authorize]
        [RoleFilter("Admin")]
        [HttpDelete("deleteevent/{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (id == 0)
            {
                return BadRequest("Error: event hasnt been deleted");
            }
            await _eventService.RemoveEvent(id);
            return Ok();
        }

        [HttpGet("events")]
        public async Task<ActionResult<IEnumerable<EventReturnDto>>> AllEvents()
        {
            var _event = await _eventService.ShowEvents();
            if (!_event.Any())
            {
                return NotFound();
            }
            return Ok(_event);
        }

    }
}
