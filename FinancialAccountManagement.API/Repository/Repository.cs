using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialAccountManagement.API.Data;
using FinancialAccountManagement.API.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace FinancialAccountManagement.API.Repository
{
    public class Repository : IRepository
    {
        private readonly FinancialAccountDbContext _context;

        public Repository(FinancialAccountDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
        {
            return await _context.Transactions
                                 .Where(t => t.AccountId == accountId)
                                 .ToListAsync();
        }

        public async Task<decimal> GetTotalBalanceAsync()
        {
            return await _context.Accounts.SumAsync(a => a.Balance);
        }

        public async Task<IEnumerable<Account>> GetAccountsBelowThresholdAsync(decimal threshold)
        {
            return await _context.Accounts
                                 .Where(a => a.Balance < threshold)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetTopAccountsByBalanceAsync(int topCount)
        {
            return await _context.Accounts
                                 .OrderByDescending(a => a.Balance)
                                 .Take(topCount)
                                 .ToListAsync();
        }
    }
}
