using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialAccountManagement.API.Model.Domain;
using FinancialAccountManagement.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAccountManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IRepository _repository;

        public StatisticsController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("transactions/{accountId}")]
        public async Task<IActionResult> GetTransactions(int accountId)
        {
            var transactions = await _repository.GetTransactionsByAccountIdAsync(accountId);
            return Ok(transactions);
        }

        [HttpGet("total-balance")]
        public async Task<IActionResult> GetTotalBalance()
        {
            var totalBalance = await _repository.GetTotalBalanceAsync();
            return Ok(totalBalance);
        }

        [HttpGet("low-balance/{threshold}")]
        public async Task<IActionResult> GetAccountsBelowThreshold(decimal threshold)
        {
            var accounts = await _repository.GetAccountsBelowThresholdAsync(threshold);
            return Ok(accounts);
        }

        [HttpGet("top-accounts/{count}")]
        public async Task<IActionResult> GetTopAccountsByBalance(int count)
        {
            var topAccounts = await _repository.GetTopAccountsByBalanceAsync(count);
            return Ok(topAccounts);
        }
    }
}
