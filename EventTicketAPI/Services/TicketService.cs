using AutoMapper;
using EventTicketAPI.Dtos.TicketSale;
using EventTicketAPI.Dtos.TicketType;
using EventTicketAPI.Entities;
using EventTicketAPI.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace EventTicketAPI.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        public TicketService(ITicketRepository ticketRepository, IMapper mapper, IDistributedCache cache)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _cache = cache;
        }
        public async Task<IEnumerable<MyTicketReturnDto>> ShowTickets(int userId)
        {
            var data = _ticketRepository.GetTickets(userId);
            if (data == null)
            {
                return null;
            }
            else
            {
                var cachekey = $"ShowTickets-{userId}";
                var cachedata = await _cache.GetStringAsync(cachekey);
                if (!string.IsNullOrEmpty(cachedata))
                {
                    return JsonConvert.DeserializeObject<IEnumerable<MyTicketReturnDto>>(cachedata);
                }
                
                var map = _mapper.Map<IEnumerable<MyTicketReturnDto>>(data);
                var cacheoptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(3)).SetAbsoluteExpiration(TimeSpan.FromHours(1));
                await _cache.SetStringAsync(cachekey, JsonConvert.SerializeObject(map), cacheoptions);
                return map;
            }
        }
        public async Task<decimal> BuyTicketService(BuyTicketDto buyTicket)
        {
            var ticket = _mapper.Map<TicketSale>(buyTicket);
            var _totalprice = await _ticketRepository.AddTicket(ticket);
            await ResetTicketsCache(buyTicket.UserId);
            await ResetTicketTypeCache(buyTicket.EventId);
            return _totalprice;
        }
        public async Task RefundTicketService(int ticketId, int userId)
        {
            _ticketRepository.DeleteTicket(ticketId,userId);
             await ResetTicketsCache(userId);
        }
        public async Task<IEnumerable<TicketTypeReturnDto>> ShowTicketTypes(int eventId)
        {
            var data = _ticketRepository.GetAllTicketTypes(eventId);
            if (data == null)
            {
                return null;
            }
            else
            {
                var cachekey = $"ShowTicketTypes-{eventId}";
                var cachedata = await _cache.GetStringAsync(cachekey);
                if (!string.IsNullOrEmpty(cachedata))
                {

                    return JsonConvert.DeserializeObject<IEnumerable<TicketTypeReturnDto>>(cachedata);
                }

                var map = _mapper.Map<IEnumerable<TicketTypeReturnDto>>(data);
                var cacheoptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)).SetAbsoluteExpiration(TimeSpan.FromHours(1));
                await _cache.SetStringAsync(cachekey, JsonConvert.SerializeObject(map), cacheoptions);
                return map;
            }

        }
        public async Task<int> AddTicketTypeService(AddTicketTypeDto addTicketType)
        {

            var map = _mapper.Map<TicketType>(addTicketType);
            var tickettype = _ticketRepository.InsertTicketType(map);

            await ResetTicketTypeCache(addTicketType.EventId);
            return tickettype;
            
        }
        public async Task UpdateTicketType(int id,AddTicketTypeDto updateTicketType)
        {
            var map = _mapper.Map<TicketType>(updateTicketType);
            map.Id = id;
            _ticketRepository.UpdateTicketTypeRepo(map);
            await ResetTicketTypeCache(updateTicketType.EventId);
        }
        public async Task RemoveTicketType(int ticketTypeId, int eventId, int userId)
        {
            _ticketRepository.DeleteTicketType(ticketTypeId, eventId);
             await ResetTicketTypeCache(eventId);
             await ResetTicketsCache(userId);
        }

        public async Task ResetTicketTypeCache(int eventId)
        {
            var cachekey = $"ShowTicketTypes-{eventId}";
            await _cache.RemoveAsync(cachekey);
        }
       public async Task ResetTicketsCache(int userid)
        {
            var cachekey = $"ShowTickets-{userid}";
            await _cache.RemoveAsync(cachekey);
        }

    }
}
