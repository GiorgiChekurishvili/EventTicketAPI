namespace EventTicketAPI.Dtos.TicketType
{
    public class AddTicketTypeDto
    {
        public required int EventId { get; set; }
        public required string TicketTypeName { get; set; }
        public required decimal Price { get; set; }
        public required int TotalTickets { get; set; }
        public required DateTime SalesStartDate { get; set; }
        public required DateTime SalesEndDate { get; set; }
    }
}
