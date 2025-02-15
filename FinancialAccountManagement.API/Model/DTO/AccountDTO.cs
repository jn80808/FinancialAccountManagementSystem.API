using System.ComponentModel.DataAnnotations;

namespace FinancialAccountManagement.API.Model.DTO
{
    public class AccountResponseDto
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountHolder { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public List<TransactionDto> Transactions { get; set; } = new();
    }


    public class AccountCreateDto
    {
        [Required, MaxLength(20)]
        public string AccountNumber { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string AccountHolder { get; set; } = string.Empty;

        public decimal Balance { get; set; }
    }

    public class AccountUpdateDto
    {
        [Required, MaxLength(100)]
        public string AccountHolder { get; set; } = string.Empty;

        public decimal Balance { get; set; }
    }



}

