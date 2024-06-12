namespace EventTicketAPI.Dtos.TicketSale
{
    public class MyTicketReturnDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public int TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public int TicketQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
