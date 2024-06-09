namespace EventTicketAPI.Dtos
{
    public class TicketTypeReturnDto
    {
        public  int id { get; set; }
        public  int EventId { get; set; }
        public required string TicketTypeName { get; set; }
        public  decimal Price { get; set; }
        public  int TicketsAvailable { get; set; }
        public  int TotalTickets { get; set; }
        public  DateTime SalesStartDate { get; set; }
        public  DateTime SalesEndDate { get; set; }
    }
}
