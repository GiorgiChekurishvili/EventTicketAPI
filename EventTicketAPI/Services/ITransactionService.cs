using EventTicketAPI.Dtos.Transactions;
using EventTicketAPI.Entities;

namespace EventTicketAPI.Services
{
    public interface ITransactionService
    {
        Task<FillTransactionsDto> MakeTransaction(FillTransactionsDto transaction);
        Task<decimal> CheckBalanceAsync(int userId);
        Task<IEnumerable<TransactionsReturnDto>> ShowMyTransactions(int userid);
        Task ResetBalanceCache(int userid);
        Task ResetTransactionsCache(int userid);
    }
}
