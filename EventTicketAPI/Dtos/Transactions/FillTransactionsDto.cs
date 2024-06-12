namespace EventTicketAPI.Dtos.Transactions
{
    public class FillTransactionsDto
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
    }
}
