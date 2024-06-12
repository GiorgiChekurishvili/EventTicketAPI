namespace EventTicketAPI.Entities
{
    public class Transactions
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public decimal Amount {  get; set; }
        public string Reason { get; set; }
        public DateTime TransactionMade { get; set; }
    }
}
