using System.ComponentModel.DataAnnotations;
using EventTicketAPI.Models;

namespace EventTicketAPI.Entities
{
    public class Event : IsDeletable
    {
       
        public int Id { get; set; }
        public required string EventName { get; set; }
        public required string EventDescription { get; set; }
        public required string EventLocation { get; set; }
        public required int Capacity { get; set; }
        public int CategoryId { get; set; }
        public required Category Category { get; set; }
        public required DateTime EventDate { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Image Image { get; set; }
        public ICollection<TicketType> TicketType { get; set; }
        public ICollection<TicketSale> TicketSale { get; set;}
        public ICollection<Favorite> Favorite { get; set; }
        

    }
}
