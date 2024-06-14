using AutoMapper;
using EventTicketAPI.Dtos.TicketSale;
using EventTicketAPI.Dtos.Transactions;
using EventTicketAPI.Entities;
using EventTicketAPI.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EventTicketAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper, IDistributedCache cache)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _cache = cache;
        }
        public async Task<decimal> CheckBalanceAsync(int userId)
        {
            decimal balance = _transactionRepository.CheckMyBalance(userId);
            if (balance == 0)
            {
                return 0;
            }
            else
            {
                var cachekey = $"CheckBalance-{userId}";
                var cachedata = await _cache.GetStringAsync(cachekey);
                if (!string.IsNullOrEmpty(cachedata))
                {
                    return JsonConvert.DeserializeObject<decimal>(cachedata);
                }
                var cacheoptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(3)).SetAbsoluteExpiration(TimeSpan.FromHours(1));
                await _cache.SetStringAsync(cachekey, JsonConvert.SerializeObject(balance), cacheoptions);
                return balance;
            }

        }

        public async  Task<FillTransactionsDto> MakeTransaction(FillTransactionsDto transaction)
        {
            var map = _mapper.Map<Transactions>(transaction);
            var result = _transactionRepository.MakeTransaction(map);
            ResetTransactionsCache(transaction.UserId);
            ResetBalanceCache(transaction.UserId);
            return _mapper.Map<FillTransactionsDto>(result);
        }

        public async Task<IEnumerable<TransactionsReturnDto>> ShowMyTransactions(int userid)
        {
            var data = _transactionRepository.ViewMyTransactions(userid);
            if (data == null)
            {
                return null;
            }
            else
            {
                var cachekey = $"ShowMyTransaction-{userid}";
                var cachedata = await _cache.GetStringAsync(cachekey);
                if (!string.IsNullOrEmpty(cachedata))
                {
                    return JsonConvert.DeserializeObject<IEnumerable<TransactionsReturnDto>>(cachedata);
                }

                var map = _mapper.Map<IEnumerable<TransactionsReturnDto>>(data);
                var cacheoptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(3)).SetAbsoluteExpiration(TimeSpan.FromHours(1));
                await _cache.SetStringAsync(cachekey, JsonConvert.SerializeObject(map), cacheoptions);

                return map;
            }
        }
        public async Task ResetBalanceCache(int userid)
        {
            var cachekey = $"CheckBalance-{userid}";
            await _cache.RemoveAsync(cachekey);
        }
        public async Task ResetTransactionsCache(int userid)
        {
            var cachekey = $"ShowMyTransaction-{userid}";
            await _cache.RemoveAsync(cachekey);
        }
    }
}
