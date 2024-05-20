using EventTicketAPI.Dtos;
using EventTicketAPI.Entities;

namespace EventTicketAPI.Services
{
    public interface ITicketService
    {
        Task<decimal> BuyTicketService(BuyTicketDto buyTicket);
        Task RefundTicketService(int eventId, int userId);
        Task<IEnumerable<MyTicketReturnDto>> ShowTickets(int userId);
        Task<int> AddTicketTypeService(AddTicketTypeDto addTicketType);
        Task UpdateTicketType(int id, AddTicketTypeDto updateTicketType);
        Task RemoveTicketType(int ticketTypeId, int eventId, int userId);
        Task<IEnumerable<TicketTypeReturnDto>> ShowTicketTypes(int eventId);
        
    }
}
