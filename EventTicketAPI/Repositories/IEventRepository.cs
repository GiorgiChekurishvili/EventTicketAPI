﻿using EventTicketAPI.Dtos;
using EventTicketAPI.Entities;
namespace EventTicketAPI.Repositories
{
    public interface IEventRepository
    {
        Event InsertEvent(Event _event);
        void UpdateEventRepo(Event _event);
        int DeleteEvent(int id);
        IEnumerable<Event> GetAllEvents();
        IEnumerable<Category> GetAllCategories();
        Favorite AddFavoriteEvent(Favorite favorite, int userid);
        void RemoveFavorite(int userId, int EventId);
        IEnumerable<Event> GetMyFavorites(int userId);
        IEnumerable<Event> GetEventByCategory(int categoryId);
        Event GetEventsById(int id);
        Task<Image> InsertImage(IFormFile fileUpload, int eventId);
        Task<Image> GetImage(int eventId);
        void DeleteImage(int eventId);


    }
}
