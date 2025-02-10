namespace FinancialAccountManagement.API.Model.Domain
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string TransactionType { get; set; } = string.Empty; // "Deposit" or "Withdrawal"
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

        // Navigation property
        public Account Account { get; set; } = null!;
    }
}
