﻿using System.ComponentModel.DataAnnotations;

namespace FinancialAccountManagement.API.Model.DTO
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }

 


}
