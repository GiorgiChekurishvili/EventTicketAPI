namespace EventTicketAPI.Dtos
{
    public class AddEventDto
    {
        public required string EventName { get; set; }
        public required string EventDescription { get; set; }
        public required string EventLocation { get; set; }
        public int Capacity { get; set; }
        public int CategoryId { get; set; }
        public DateTime EventDate { get; set; }
    }
}
