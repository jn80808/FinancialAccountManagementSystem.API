using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialAccountManagement.API.Model.Domain
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountHolder { get; set; } = string.Empty;
        public decimal Balance { get; set; }

        // Navigation property
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

}
