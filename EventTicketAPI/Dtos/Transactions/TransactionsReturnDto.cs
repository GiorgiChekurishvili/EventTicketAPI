using System.ComponentModel.DataAnnotations;

namespace EventTicketAPI.Dtos.Transactions
{
    public class TransactionsReturnDto
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Reason { get; set; }
        [Required]
        public decimal BalanceChanges { get; set; }
        [Required]
        public DateTime TransactionMade { get; set; }

    }
}
