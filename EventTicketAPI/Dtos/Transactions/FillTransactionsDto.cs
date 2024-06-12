using System.ComponentModel.DataAnnotations;

namespace EventTicketAPI.Dtos.Transactions
{
    public class FillTransactionsDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Reason { get; set; }
    }
}
