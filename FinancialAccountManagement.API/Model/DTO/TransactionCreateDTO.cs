using System.ComponentModel.DataAnnotations;

namespace FinancialAccountManagement.API.Model.DTO
{
    public class TransactionCreateDto
    {
        [Required(ErrorMessage = "AccountId is required.")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "TransactionType is required.")]
        [RegularExpression("Deposit|Withdrawal", ErrorMessage = "TransactionType must be either 'Deposit' or 'Withdrawal'")]
        public string TransactionType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}
