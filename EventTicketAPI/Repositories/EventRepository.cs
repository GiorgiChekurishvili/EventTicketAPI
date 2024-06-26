﻿using EventTicketAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace EventTicketAPI.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly EventTicketContext _context;
        public EventRepository(EventTicketContext context)
        {

            _context = context;

        }
        public Event InsertEvent(Event _event)
        {
            if (_event == null)
            {
                return null;
            }
            if (_event.EventDate <= DateTime.Now)
            {
                return null;
            }
            var ifexists = _context.Events.FirstOrDefault(x=>x.EventName.ToLower() == _event.EventName.ToLower());
            if (ifexists == null)
            {
                if (_event.Capacity == 0)
                {
                    return null;
                }
                _context.Events.Add(_event);
                _context.SaveChanges();

                return _event;
            }
            else
            {
                return null;
            }

        }
        public void UpdateEventRepo(Event _event)
        {

            if (_event != null)
            {
                if (_event.EventDate <= DateTime.Now)
                {
                    return;
                }


                if (_event.Capacity != 0)
                {
                    _context.Events.Entry(_event).State = EntityState.Modified;
                    _context.SaveChanges();

                }

            }
        }
        public int DeleteEvent(int id)
        {
            var _event = _context.Events.FirstOrDefault(e => e.Id == id);
            if (_event != null)
            {
                var tickettypes = _context.TicketTypes.Where(x => x.EventId == id).ToList();
                for (int i = 0; i < tickettypes.Count; i++)
                {
                    _context.TicketTypes.Remove(tickettypes[i]);
                }
                
                var ticketsales = _context.TicketSales.Where(x=>x.EventId == id).ToList();
                for (int i = 0; i < ticketsales.Count; i++)
                {
                    _context.TicketSales.Remove(ticketsales[i]);
                }
               
                _context.Events.Remove(_event);
                _context.SaveChanges();
                return _event.CategoryId;
            }
            return 0;
            
        }
        public IEnumerable<Event> GetAllEvents()
        {
           
            var events = _context.Events.ToList();
            return events;
        }
        public Event GetEventsById(int id)
        {
            var events = _context.Events.Find(id);
            return events;
        }
        public IEnumerable<Category> GetAllCategories()
        {
            
            var categories = _context.Categories.ToList();
            return categories;
        }
        public Favorite AddFavoriteEvent(Favorite favorite, int userid)
        {
            var ifexists = _context.Favorites.FirstOrDefault(x=>x.EventId == favorite.EventId && x.UserId == userid);
            if (ifexists != null)
            {
                return null;
            }

            _context.Favorites.Add(favorite);
            _context.SaveChanges();
            return favorite;
        }

        public void RemoveFavorite(int userId, int EventId)
        {
            var _event = _context.Favorites.Where(x=> x.UserId == userId).ToList();
            if (_event != null)
            {
                var removevent = _event.FirstOrDefault(x => x.EventId == EventId);
                if (removevent != null)
                {
                    _context.Favorites.Remove(removevent);
                    _context.SaveChanges();

                }
                
            }
        }

        public IEnumerable<Event> GetMyFavorites(int userId)
        {
            var favorites = _context.Favorites.Where(x=>x.UserId == userId).Select(x=>x.EventId).ToList();
            var eventdata = _context.Events.Where(x=> favorites.Contains(x.Id)).ToList();

            return eventdata;

        }

        public IEnumerable<Event> GetEventByCategory(int categoryId)
        {
            var events = _context.Events.Where(x => x.CategoryId == categoryId).ToList();
            if (events == null)
            {
                return null;
            }
            return events;
        }
        public async Task<Image> InsertImage(IFormFile fileUpload, int eventId)
        {
            try
            {
                if (fileUpload.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await fileUpload.CopyToAsync(memoryStream);

                        var image = new Image
                        {
                            EventId = eventId,
                            Photo = memoryStream.ToArray(),
                            ImageType = fileUpload.ContentType
                        };

                        var eventEntity = await _context.Events.Include(e => e.Image).FirstOrDefaultAsync(e => e.Id == eventId);
                        if (eventEntity == null)
                        {
                            return null;
                        }

                        if (eventEntity.Image == null)
                        {
                            _context.Images.Add(image);
                        }
                        else
                        {
                            eventEntity.Image.Photo = image.Photo;
                            eventEntity.Image.ImageType = image.ImageType;
                            _context.Images.Update(eventEntity.Image);
                        }

                        await _context.SaveChangesAsync();
                        return image;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async  Task<Image> GetImage(int eventId)
        {
            var eventEntity = await _context.Images.FirstOrDefaultAsync(e => e.EventId == eventId);

            if (eventEntity == null || eventEntity.Photo == null)
            {
                return null;
            }

            return eventEntity;
        }
        public void DeleteImage(int eventId)
        {
            var image = _context.Images.FirstOrDefault(x => x.EventId == eventId);
            if (image != null)
            {
                _context.Images.Remove(image);
                _context.SaveChanges();
            }
            


        }
    }
}
