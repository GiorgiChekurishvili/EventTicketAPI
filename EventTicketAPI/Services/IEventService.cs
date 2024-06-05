using EventTicketAPI.Dtos;
using EventTicketAPI.Entities;

namespace EventTicketAPI.Services
{
    public interface IEventService
    {
        Task<AddEventDto> AddEventService(AddEventDto addEvent);
        Task UpdateEvent(int id, AddEventDto updateEvent);
        Task RemoveEvent(int id);
        Task<IEnumerable<EventReturnDto>> ShowEvents();
        Task<IEnumerable<CategoryReturnDto>> ShowCategories();
        Task<Favorite> AddFavoriteEvent(Favorite favorite, int userid);
        Task UnfavoriteEvent(int userId, int EventId);
        Task<IEnumerable<EventReturnDto>> ShowMyFavorites(int userid);
        Task<IEnumerable<EventReturnDto>> ShowEventsByCategory(int  categoryid);
        Task<EventReturnDto> ShowEventsById(int id);
    }
}
