namespace FinancialAccountManagement.API.Model.DTO
{
    public class TransactionCreateDTO
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
