using EventTicketAPI.Dtos.Transactions;
using EventTicketAPI.Entities;

namespace EventTicketAPI.Services
{
    public interface ITransactionService
    {
        FillTransactionsDto MakeTransaction(FillTransactionsDto transaction);
        decimal CheckBalance(int userId);
        IEnumerable<TransactionsReturnDto> ShowMyTransactions(int userid);
    }
}
