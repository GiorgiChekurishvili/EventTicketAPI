namespace EventTicketAPI.Entities
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }
}
