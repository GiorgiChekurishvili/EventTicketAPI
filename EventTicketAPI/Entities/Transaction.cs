namespace EventTicketAPI.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public decimal Amount {  get; set; }
        public decimal Balance { get; set; }
        public DateTime TransactionMade { get; set; }
    }
}
