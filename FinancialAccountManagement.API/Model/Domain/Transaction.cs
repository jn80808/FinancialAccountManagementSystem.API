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

        [Required]
        [RegularExpression("Deposit|Withdrawal", ErrorMessage = "TransactionType must be either 'Deposit' or 'Withdrawal'")]
        public string TransactionType { get; set; } = string.Empty; // "Deposit" or "Withdrawal"

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Transaction amount must be greater than zero")]
        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public Account? Account { get; set; } //Navigation property
    }
}
