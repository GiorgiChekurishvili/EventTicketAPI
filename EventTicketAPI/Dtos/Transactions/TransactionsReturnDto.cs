namespace EventTicketAPI.Dtos.Transactions
{
    public class TransactionsReturnDto
    {
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public DateTime TransactionMade { get; set; }
    }
}
