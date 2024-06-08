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
        [HttpGet("eventsbycategory/{categoryid}")]
        public async Task<ActionResult<IEnumerable<EventReturnDto>>> EventsByCategory(int categoryid)
        {
            var categories = await _eventService.ShowEventsByCategory(categoryid);
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
            if (addEvent.EventDate < currentdate)
            {
                return Conflict("Datetime is incorrect");
            }
            if (addEvent.Capacity < 0)
            {
                return Forbid("capacity must be greater than 0");
            }
            var _event = await _eventService.AddEventService(addEvent);
            if (_event == null)
            {
                return BadRequest("Event's name already exists");
            }
            
            return Ok("Event has been already added");
        }

        [Authorize]
        [RoleFilter("Admin")]
        [HttpPut("updateevent/{id}")]
        public async Task<IActionResult> UpdateEvent(int id, AddEventDto addEvent)
        {
            var currentdate = DateTime.Now;
            if (addEvent.EventDate < currentdate)
            {
                return BadRequest("Datetime is incorrect");
            }
            if (addEvent.Capacity < 0)
            {
                return Forbid("capacity mut be greater than 0");
            }
            await _eventService.UpdateEvent(id, addEvent);
            return Ok();
        }

        [Authorize]
        [RoleFilter("Admin")]
        [HttpDelete("deleteevent/{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
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
        [HttpGet("eventsbyid/{id}")]
        public async Task<ActionResult<EventReturnDto>> EventsById(int id)
        {
            var _eventsbyid = await _eventService.ShowEventsById(id);
            if (_eventsbyid == null)
            {
                return NotFound();
            }
            return Ok(_eventsbyid);
        }

    }
}
