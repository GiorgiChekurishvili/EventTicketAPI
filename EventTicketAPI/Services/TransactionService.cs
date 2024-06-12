using AutoMapper;
using EventTicketAPI.Dtos.Transactions;
using EventTicketAPI.Entities;
using EventTicketAPI.Repositories;

namespace EventTicketAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }
        public decimal CheckBalance(int userId)
        {
            decimal balance = _transactionRepository.CheckMyBalance(userId);
            return balance;
        }

        public FillTransactionsDto MakeTransaction(FillTransactionsDto transaction)
        {
            var map = _mapper.Map<Transactions>(transaction);
            var result = _transactionRepository.MakeTransaction(map);
            
            return _mapper.Map<FillTransactionsDto>(result);
        }

        public IEnumerable<TransactionsReturnDto> ShowMyTransactions(int userid)
        {
            var data = _transactionRepository.ViewMyTransactions(userid);
            var map = _mapper.Map<IEnumerable<TransactionsReturnDto>>(data);
            return map;
        }
    }
}
