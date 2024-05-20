using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventTicketAPI.Entities
{
    public class TicketType : ISoftDeletable
    {
       
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public required string TicketTypeName { get; set; }
        public required decimal Price { get; set; }
        public required int TicketsAvailable { get; set; }
        public required int TotalTickets { get; set; }
        public required DateTime SalesStartDate { get; set; }
        public required DateTime SalesEndDate { get; set;}
        public DateTime DateAdded { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<TicketSale> TicketSale { get; set; }
    }
}
