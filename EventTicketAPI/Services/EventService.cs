using AutoMapper;
using EventTicketAPI.Dtos;
using EventTicketAPI.Entities;
using EventTicketAPI.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EventTicketAPI.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        
        public EventService(IEventRepository eventRepository, IMapper mapper, IDistributedCache cache)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _cache = cache;
        }
        public async Task<AddEventDto> AddEventService(AddEventDto addEvent)
        {
            var map = _mapper.Map<Event>(addEvent);
            var _event =_eventRepository.InsertEvent(map);
            
            await ResetEventsCache();
            await ResetEventsByCategoryCache(addEvent.CategoryId);
            try
            {


                await ResetEventsByIdCache(_event.Id);
            }
            catch
            {
                return null;
            }
            return _mapper.Map<AddEventDto>(_event);

        }

        public async Task RemoveEvent(int id)
        {
            var categoryid = _eventRepository.DeleteEvent(id);
            await ResetEventsByCategoryCache(categoryid);
            await ResetEventsCache();
            await ResetEventsByIdCache(id);
        }
        public async Task UpdateEvent(int id, AddEventDto updateEvent)
        {
            var map = _mapper.Map<Event>(updateEvent);
            map.Id = id;
            _eventRepository.UpdateEventRepo(map);
            await ResetEventsCache();
            await ResetEventsByCategoryCache(updateEvent.CategoryId);
            await ResetEventsByIdCache(id);
        }
        
        public async Task<IEnumerable<EventReturnDto>> ShowEvents()
        {
            var cachekey = "ShowEvents";
            var cachedata = await _cache.GetStringAsync(cachekey);
            if (!string.IsNullOrEmpty(cachedata))
            {
                return JsonConvert.DeserializeObject<IEnumerable<EventReturnDto>>(cachedata);
            }

            var events = _eventRepository.GetAllEvents();
            var map = _mapper.Map<IEnumerable<EventReturnDto>>(events);
            var cacheoptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(30)).SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
            await _cache.SetStringAsync(cachekey,JsonConvert.SerializeObject(map), cacheoptions);
            return map;
        }
        public async Task<EventReturnDto> ShowEventsById(int id)
        {
            var cachekey = $"ShowEventsById-{id}";
            var cachedata = await _cache.GetStringAsync(cachekey);
            if (!string.IsNullOrEmpty(cachedata))
            {
                return JsonConvert.DeserializeObject<EventReturnDto>(cachedata);
            }
            var events = _eventRepository.GetEventsById(id);
            var map = _mapper.Map<EventReturnDto>(events);
            var cacheoptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(3)).SetAbsoluteExpiration(TimeSpan.FromHours(1));
            await _cache.SetStringAsync(cachekey, JsonConvert.SerializeObject(map), cacheoptions);
            return map;
        }
        public async Task<IEnumerable<CategoryReturnDto>> ShowCategories()
        {
            var cachekey = "ShowCaregories";
            var cachedata = await _cache.GetStringAsync(cachekey);
            if (!string.IsNullOrEmpty(cachedata))
            {
                return JsonConvert.DeserializeObject<IEnumerable<CategoryReturnDto>>(cachedata);
            }
            var categories = _eventRepository.GetAllCategories();
            var map = _mapper.Map<IEnumerable<CategoryReturnDto>>(categories);
            var cacheoptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)).SetAbsoluteExpiration(TimeSpan.FromHours(1));
            await _cache.SetStringAsync(cachekey, JsonConvert.SerializeObject(map), cacheoptions);
            return map; 
        }

        public async Task<Favorite> AddFavoriteEvent(Favorite favorite, int userid)
        {
            var ticket = _eventRepository.AddFavoriteEvent(favorite, userid);
            if (ticket == null)
            {
                return null;
            }
            await ResetFavoritesCache(favorite.UserId);
            return ticket;
           
        }
        public async Task<IEnumerable<EventReturnDto>> ShowMyFavorites(int userid)
        {
            var cachekey = $"ShowMyFavorites-{userid}";
            var cachedata = await _cache.GetStringAsync(cachekey);
            if (!string.IsNullOrEmpty(cachedata))
            {
                return JsonConvert.DeserializeObject<IEnumerable<EventReturnDto>>(cachedata);
            }
            var data = _eventRepository.GetMyFavorites(userid);
            var map = _mapper.Map<IEnumerable<EventReturnDto>>(data);
            var cacheoptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)).SetAbsoluteExpiration(TimeSpan.FromHours(1));
            await _cache.SetStringAsync(cachekey,JsonConvert.SerializeObject(map), cacheoptions);
            return map;
            
        }
        public async Task<IEnumerable<EventReturnDto>> ShowEventsByCategory(int categoryid)
        {
            var cachekey = $"ShowEventsByCategory-{categoryid}";
            var cachedata = await _cache.GetStringAsync(cachekey);
            if (!string.IsNullOrEmpty(cachedata))
            {
                return JsonConvert.DeserializeObject<IEnumerable<EventReturnDto>>(cachedata);
            }
            var events = _eventRepository.GetEventByCategory(categoryid);
            var map = _mapper.Map<IEnumerable<EventReturnDto>>(events);
            var cacheoptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)).SetSlidingExpiration(TimeSpan.FromHours(1));
            await _cache.SetStringAsync(cachekey, JsonConvert.SerializeObject(map),cacheoptions);
            return map;
        }
        public async Task UnfavoriteEvent(int userId, int EventId)
        {
            _eventRepository.RemoveFavorite(userId, EventId);
            await ResetFavoritesCache(userId);
        }

        
        public async Task ResetEventsByCategoryCache(int categoryid)
        {
            var cachekey = $"ShowEventsByCategory-{categoryid}";
            await _cache.RemoveAsync(cachekey);
        }
        public async Task ResetEventsCache()
        {
            var cachekey = "ShowEvents";
            await _cache.RemoveAsync(cachekey);
        }
        public async Task ResetFavoritesCache(int userid)
        {
            var cachekey = $"ShowMyFavorites-{userid}";
            await _cache.RemoveAsync(cachekey);
        }
        public async Task ResetEventsByIdCache(int eventid)
        {
            var cachekey = $"ShowEventsById-{eventid}";
            await _cache.RemoveAsync(cachekey);
        }


    }
}
