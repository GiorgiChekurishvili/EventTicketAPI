namespace EventTicketAPI.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public byte[] Photo { get; set; }
        public string ImageType { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }

    }
}
