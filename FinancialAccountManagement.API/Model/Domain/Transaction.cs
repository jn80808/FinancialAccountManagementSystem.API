using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FinancialAccountManagement.API.Model.Domain
{

    [Table("Transaction")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public string TransactionType { get; set; } = string.Empty; // "Deposit" or "Withdrawal"
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

        [JsonIgnore]
        public Account? Account { get; set; } = null!; // Navigation property
    }
}
