using System.Transactions;
using EventTicketAPI.Entities;

namespace EventTicketAPI.Repositories
{
    public interface ITransactionRepository
    {
        Transactions MakeTransaction(Transactions transaction);
        decimal CheckMyBalance(int userId);
        IEnumerable<Transactions> ViewMyTransactions(int userid);
    }
}
