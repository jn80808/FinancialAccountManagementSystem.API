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
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] AccountUpdateDto dto)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return NotFound();

            if (dto.Balance < 0) return BadRequest("Balance cannot be negative.");

            account.AccountHolder = dto.AccountHolder;
            account.Balance = dto.Balance;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/account/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _context.Accounts.Include(a => a.Transactions).FirstOrDefaultAsync(a => a.Id == id);
            if (account == null) return NotFound();

            if (account.Transactions.Any())
            {
                return BadRequest("Cannot delete account with existing transactions.");
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
