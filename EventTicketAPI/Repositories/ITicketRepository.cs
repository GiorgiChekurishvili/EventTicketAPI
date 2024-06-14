using EventTicketAPI.Dtos;
using EventTicketAPI.Entities;

namespace EventTicketAPI.Repositories
{
    public interface ITicketRepository
    {
        Task<decimal> AddTicket(TicketSale ticketSale);
        void DeleteTicket(int eventId, int userId);
        IEnumerable<TicketSale> GetTickets(int userId);
        int InsertTicketType(TicketType ticketType);
        void UpdateTicketTypeRepo(TicketType ticketType);
        void DeleteTicketType(int ticketTypeId, int eventId);
        IEnumerable<TicketType> GetAllTicketTypes(int eventId);
    }
}
