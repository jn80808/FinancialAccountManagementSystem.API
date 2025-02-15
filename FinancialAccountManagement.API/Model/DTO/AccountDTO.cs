namespace FinancialAccountManagement.API.Model.DTO
{
    public class AccountDTO
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountHolder { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public List<TransactionDTO> Transactions { get; set; } = new();
    }
}

