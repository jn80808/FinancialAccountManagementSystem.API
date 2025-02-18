using FinancialAccountManagement.API.Data;
using FinancialAccountManagement.API.Model.Domain;
using FinancialAccountManagement.API.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialAccountManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly FinancialAccountDbContext _context;

        public TransactionController(FinancialAccountDbContext context)
        {
            _context = context;
        }

        //  GET: api/transactions - Retrieve all transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions()
        {
            var transactions = await _context.Transactions.ToListAsync();

            return transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                AccountId = t.AccountId,
                TransactionType = t.TransactionType,
                Amount = t.Amount,
                TransactionDate = t.TransactionDate
            }).ToList();
        }

        // GET: api/transactions/{id} - Retrieve a specific transaction by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetTransactionById(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) return NotFound();

            return new TransactionDto
            {
                Id = transaction.Id,
                AccountId = transaction.AccountId,
                TransactionType = transaction.TransactionType,
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate
            };
        }

        // GET: api/transactions/account/{accountId} - Retrieve transactions by AccountId
        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsByAccount(int accountId)
        {
            var transactions = await _context.Transactions.Where(t => t.AccountId == accountId).ToListAsync();
            if (!transactions.Any()) return NotFound();

            return transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                AccountId = t.AccountId,
                TransactionType = t.TransactionType,
                Amount = t.Amount,
                TransactionDate = t.TransactionDate
            }).ToList();
        }

        // POST: api/transactions - Create a new transaction (Deposit or Withdrawal)
        [HttpPost]
        public async Task<ActionResult<TransactionDto>> CreateTransaction(TransactionCreateDto dto)
        {
            var account = await _context.Accounts.FindAsync(dto.AccountId);
            if (account == null) return NotFound("Account not found.");

            // Handle the business logic for Withdrawals
            if (dto.TransactionType == "Withdrawal")
            {
                if (account.Balance < dto.Amount)
                    return BadRequest("Insufficient balance for withdrawal.");

                account.Balance -= dto.Amount;  // Subtract the amount from the account balance
            }

            // Handle the business logic for Deposits
            if (dto.TransactionType == "Deposit")
            {
                account.Balance += dto.Amount;  // Add the amount to the account balance
            }

            var transaction = new Transaction
            {
                AccountId = dto.AccountId,
                TransactionType = dto.TransactionType,
                Amount = dto.Amount,
                TransactionDate = DateTime.UtcNow
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, new TransactionDto
            {
                Id = transaction.Id,
                AccountId = transaction.AccountId,
                TransactionType = transaction.TransactionType,
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate
            });
        }

        // PUT: api/transactions/{id} - Update an existing transaction
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, TransactionCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid request. Transaction data is required.");

            if (dto.Amount <= 0)
                return BadRequest("Amount must be greater than zero.");

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
                return NotFound($"Transaction with ID {id} not found.");

            var account = await _context.Accounts.FindAsync(dto.AccountId);
            if (account == null)
                return NotFound($"Account with ID {dto.AccountId} not found.");

            // Calculate new balance by reversing the original transaction
            decimal newBalance = account.Balance;

            if (transaction.TransactionType == "Deposit")
                newBalance -= transaction.Amount;
            else if (transaction.TransactionType == "Withdrawal")
                newBalance += transaction.Amount;

            // Fix: Only block if reversing a withdrawal results in a negative balance
            if (transaction.TransactionType == "Withdrawal" && newBalance < 0)
                return BadRequest($"Reversing the previous withdrawal would cause a negative balance: {newBalance}");

            // Validate new withdrawal before applying the update
            if (dto.TransactionType == "Withdrawal" && newBalance < dto.Amount)
                return BadRequest($"Insufficient balance after update. Available balance: {newBalance}, requested withdrawal: {dto.Amount}");

            // Apply the new transaction effect on balance
            if (dto.TransactionType == "Deposit")
                newBalance += dto.Amount;
            else if (dto.TransactionType == "Withdrawal")
                newBalance -= dto.Amount;

            // Final validation before saving
            if (newBalance < 0)
                return BadRequest($"Transaction update would result in a negative balance: {newBalance}");

            // Update the transaction details and save
            transaction.TransactionType = dto.TransactionType;
            transaction.Amount = dto.Amount;
            account.Balance = newBalance;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Transaction updated successfully.", transactionId = transaction.Id, newBalance });
        }




        // DELETE: api/transactions/{id} - Delete a transaction
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) return NotFound();

            var account = await _context.Accounts.FindAsync(transaction.AccountId);
            if (account == null) return NotFound("Account not found.");

            // Reverse transaction effect before deleting
            if (transaction.TransactionType == "Deposit")
                account.Balance -= transaction.Amount;
            else if (transaction.TransactionType == "Withdrawal")
                account.Balance += transaction.Amount;

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Transaction deleted successfully and account balance updated." });
        }
    }
}
