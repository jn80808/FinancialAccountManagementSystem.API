using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialAccountManagement.API.Model.Domain;

namespace FinancialAccountManagement.API.Repository
{
    public interface IRepository
    {
        Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId);
        Task<decimal> GetTotalBalanceAsync();
        Task<IEnumerable<Account>> GetAccountsBelowThresholdAsync(decimal threshold);
        Task<IEnumerable<Account>> GetTopAccountsByBalanceAsync(int topCount);
    }
}
