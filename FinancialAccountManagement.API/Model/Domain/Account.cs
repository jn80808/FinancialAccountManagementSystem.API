using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialAccountManagement.API.Model.Domain
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string AccountNumber { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string AccountHolder { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Balance cannot be negative")]
        public decimal Balance { get; set; }

        public ICollection<Transaction>? Transactions { get; set; } = new List<Transaction>();
    }
}
