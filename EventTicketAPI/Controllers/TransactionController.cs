﻿using EventTicketAPI.Dtos.Transactions;
using EventTicketAPI.Filter;
using EventTicketAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventTicketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }


        
        [Authorize]
        [RoleFilter("member")]
        [HttpPost("maketransaction/{amount}")]
        public async Task<IActionResult> MakeTransaction(decimal amount)
        {
            var user = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            FillTransactionsDto fillTransactionsDto = new FillTransactionsDto()
            {
                UserId = user,
                Amount = amount,
                Reason = "Added Cash To Balance"
            };
            var transaction = await _transactionService.MakeTransaction(fillTransactionsDto);
            if (transaction == null)
            {
                return BadRequest();
            }
            return Ok();
        }
        
        [Authorize]
        [RoleFilter("member")]
        [HttpGet("viewmybalance")]
        public async Task<ActionResult<int>> ViewMyBalance()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var balance = await _transactionService.CheckBalanceAsync(userId);
            return Ok(balance);
        }
        
        [Authorize]
        [RoleFilter("member")]
        [HttpGet("viewmytransactions")]
        public async Task<ActionResult<IEnumerable<TransactionsReturnDto>>> ViewMyTransactions()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var transactions = await _transactionService.ShowMyTransactions(userId);
            if (transactions == null)
            {
                return BadRequest();
            }
            return Ok(transactions);
        }
    }
}
