using FinancialAccountManagement.API.Repository;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly IRepository _repository;

    // Add constructor for dependency injection
    public ReportsController(IRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("transactions/{accountId}")]
    public async Task<IActionResult> GetTransactions(int accountId)
    {
        if (accountId <= 0)
        {
            return BadRequest(new { message = "Invalid account ID. It must be greater than zero." });
        }

        var accountExists = await _repository.DoesAccountExistAsync(accountId);
        if (!accountExists)
        {
            return NotFound(new { message = $"Account with ID {accountId} does not exist." });
        }

        var transactions = await _repository.GetTransactionsByAccountIdAsync(accountId);

        if (transactions == null || !transactions.Any())
        {
            return NotFound(new { message = $"No transactions found for account ID {accountId}." });
        }

        return Ok(new { message = "Transactions retrieved successfully.", transactions });
    }

    [HttpGet("total-balance")]
    public async Task<IActionResult> GetTotalBalance()
    {
        var totalBalance = await _repository.GetTotalBalanceAsync();
        return Ok(new { message = "Total balance retrieved successfully.", totalBalance });
    }

    [HttpGet("low-balance/{threshold}")]
    public async Task<IActionResult> GetAccountsBelowThreshold(decimal threshold)
    {
        if (threshold < 0)
        {
            return BadRequest(new { message = "Threshold value must be a positive number." });
        }

        var accounts = await _repository.GetAccountsBelowThresholdAsync(threshold);

        if (!accounts.Any())
        {
            return NotFound(new { message = $"No accounts found below the threshold of {threshold}." });
        }

        return Ok(new { message = "Accounts retrieved successfully.", accounts });
    }

    [HttpGet("top-accounts/{count}")]
    public async Task<IActionResult> GetTopAccountsByBalance(int count)
    {
        if (count <= 0)
        {
            return BadRequest(new { message = "Count must be greater than zero." });
        }

        var topAccounts = await _repository.GetTopAccountsByBalanceAsync(count);

        if (!topAccounts.Any())
        {
            return NotFound(new { message = "No accounts found." });
        }

        return Ok(new { message = $"Top {count} accounts retrieved successfully.", topAccounts });
    }
}
