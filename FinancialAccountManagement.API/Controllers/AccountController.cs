using FinancialAccountManagement.API.Data;
using FinancialAccountManagement.API.Model.Domain;
using FinancialAccountManagement.API.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialAccountManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly FinancialAccountDbContext _context;

        public AccountController(FinancialAccountDbContext context)
        {
            _context = context;
        }

        // GET: api/account
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountResponseDto>>> GetAccounts()
        {
            var accounts = await _context.Accounts
                .Include(a => a.Transactions)
                .AsNoTracking() // Optimization for read operations
                .ToListAsync();

            var accountDtos = accounts.Select(a => new AccountResponseDto
            {
                Id = a.Id,
                AccountNumber = a.AccountNumber,
                AccountHolder = a.AccountHolder,
                Balance = a.Balance,
                Transactions = a.Transactions.Select(t => new TransactionDto
                {
                    Id = t.Id,
                    AccountId = t.AccountId,
                    TransactionType = t.TransactionType,
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate
                }).ToList()
            }).ToList();

            return Ok(accountDtos);
        }

        // GET: api/account/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountResponseDto>> GetAccount(int id)
        {
            var account = await _context.Accounts
                .Include(a => a.Transactions)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (account == null) return NotFound();

            return Ok(new AccountResponseDto
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                AccountHolder = account.AccountHolder,
                Balance = account.Balance,
                Transactions = account.Transactions.Select(t => new TransactionDto
                {
                    Id = t.Id,
                    AccountId = t.AccountId,
                    TransactionType = t.TransactionType,
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate
                }).ToList()
            });
        }

        // POST: api/account
        [HttpPost]
        public async Task<ActionResult<AccountResponseDto>> CreateAccount([FromBody] AccountCreateDto dto)
        {
            if (dto.Balance < 0) return BadRequest("Balance cannot be negative.");

            var account = new Account
            {
                AccountNumber = dto.AccountNumber,
                AccountHolder = dto.AccountHolder,
                Balance = dto.Balance
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, new AccountResponseDto
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                AccountHolder = account.AccountHolder,
                Balance = account.Balance,
                Transactions = new List<TransactionDto>()
            });
        }

        // PUT: api/account/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] AccountUpdateDto dto, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return NotFound();

            if (string.IsNullOrWhiteSpace(dto.AccountHolder))
                return BadRequest("Account holder name cannot be empty.");

            if (dto.Balance < 0) return BadRequest("Balance cannot be negative.");

            account.AccountHolder = dto.AccountHolder;
            account.Balance = dto.Balance;

            await _context.SaveChangesAsync(cancellationToken);

            return Ok(new AccountResponseDto
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                AccountHolder = account.AccountHolder,
                Balance = account.Balance,
                Transactions = new List<TransactionDto>() // Include as needed
            });
        }


        // DELETE: api/account/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts
                .Where(a => a.Id == id)
                .Select(a => new { a.Id, HasTransactions = a.Transactions.Any() }) // Only fetch needed data
                .FirstOrDefaultAsync(cancellationToken);

            if (account == null)
            {
                return NotFound(new { message = "Account does not exist." });
            }

            if (account.HasTransactions)
            {
                return BadRequest(new { message = "Cannot delete account with existing transactions." });
            }

            // Retrieve the account again (EF cannot delete from projection)
            var accountToDelete = new Account { Id = id };
            _context.Accounts.Attach(accountToDelete);
            _context.Accounts.Remove(accountToDelete);

            await _context.SaveChangesAsync(cancellationToken);

            return Ok(new { message = "Account successfully deleted." });
        }
    }
}