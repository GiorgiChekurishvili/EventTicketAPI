using EventTicketAPI.Entities;
using System.Transactions;

namespace EventTicketAPI.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        

        private readonly EventTicketContext _context;
        public TransactionRepository(EventTicketContext context)
        {
            _context = context;
        }
        public decimal CheckMyBalance(int userId)
        {
            var balance = _context.Users.FirstOrDefault(x=>x.Id == userId);
            if (balance != null)
            {
                return balance.Balance;
            }
            return 0;
            
        }

        public Transactions MakeTransaction(Transactions transaction)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == transaction.UserId);
            if (transaction.Amount == 0)
            {
                return null;
            }

            if (transaction.Amount < 0)
            {
                if (user.Balance < transaction.Amount * -1)
                {
                    return null;
                }
            }
            
            if (user != null)
            {

                user.Balance += transaction.Amount;
                if (user.Balance < 0)
                {
                    return null;
                }
                
                transaction.BalanceChanges = user.Balance + transaction.BalanceChanges;
                _context.Users.Update(user);
                _context.SaveChanges();
                _context.Transactions.Add(transaction);
                _context.SaveChanges();
                return transaction;

            }
            return null;
            



        }

        public IEnumerable<Transactions> ViewMyTransactions(int userid)
        {
            var history = _context.Transactions.Where(x=>x.UserId ==  userid).ToList();
            if (history.Count > 0)
            {
                return history;
            }
            return null;

        }
    }
}
