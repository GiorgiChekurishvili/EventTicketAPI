namespace EventTicketAPI.Dtos.TicketSale
{
    public class BuyTicketDto
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public required int TicketTypeId { get; set; }
        public required int TicketQuantity { get; set; }
    }
}
