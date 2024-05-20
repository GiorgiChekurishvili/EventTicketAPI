namespace EventTicketAPI.Dtos
{
    public class MyTicketReturnDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public  int TicketTypeId { get; set; }
        public  int TicketQuantity { get; set; }
        public int TotalPrice { get; set; }
    }
}
