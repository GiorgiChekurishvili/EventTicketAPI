using System.ComponentModel.DataAnnotations;

namespace EventTicketAPI.Entities
{
    public class TicketSale : IsDeletable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public required int TicketTypeId { get; set; }
        public TicketType TicketType { get; set; }
        public required int TicketQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}