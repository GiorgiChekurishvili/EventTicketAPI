namespace EventTicketAPI.Dtos.Event
{
    public class EventReturnDto
    {
        public int Id { get; set; }
        public required string EventName { get; set; }
        public required string EventDescription { get; set; }
        public required string EventLocation { get; set; }
        public required int Capacity { get; set; }
        public int CategoryId { get; set; }
        public DateTime EventDate { get; set; }
    }
}
