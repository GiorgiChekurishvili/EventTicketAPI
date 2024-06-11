using EventTicketAPI.Entities;
using EventTicketAPI.Filter;
using EventTicketAPI.Models;
using EventTicketAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;



namespace EventTicketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        public static IWebHostEnvironment _webHostEnvironment;
        private readonly IEventService _eventService;
        public ImagesController(IWebHostEnvironment webHostEnvironment, IEventService eventService)
        {
            _webHostEnvironment = webHostEnvironment;
            _eventService = eventService;
        }
        [Authorize]
        [RoleFilter("Admin")]
        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage([FromForm]FileManagement fileUpload, int eventId)
        {
            if (fileUpload == null || fileUpload.files.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            await _eventService.AddImage(fileUpload.files, eventId);
            

            return Ok();
        }
        [HttpGet("RetrieveImage/{eventId}")]
        public async Task<IActionResult> RetrieveImage(int eventId)
        {
            var eventDto = await _eventService.ShowImage(eventId);
            if (eventDto == null)
            {
                return NotFound("Event not found or does not have an associated image.");
            }

            return File(eventDto.Photo, eventDto.ImageType);
        }
        [HttpDelete("DeleteImage/{eventId}")]
        public IActionResult DeleteImage(int eventId)
        {
            _eventService.RemoveImage(eventId);
            return Ok();
        }


    }
}
