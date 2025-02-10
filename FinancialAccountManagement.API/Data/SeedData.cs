using FinancialAccountManagement.API.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace FinancialAccountManagement.API.Data
{
    public class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Seed initial accounts
            modelBuilder.Entity<Account>().HasData(
                new Account { Id = 1, AccountNumber = "ACC123", AccountHolder = "Chiaki Kobayashi", Balance = 1000.00m },
                new Account { Id = 2, AccountNumber = "ACC456", AccountHolder = "Rie Takahashi", Balance = 2000.00m },
                new Account { Id = 3, AccountNumber = "ACC789", AccountHolder = "Yumiri Hanamori", Balance = 1500.00m },
                new Account { Id = 4, AccountNumber = "ACC101", AccountHolder = "Makoto Koichi", Balance = 3000.00m },
                new Account { Id = 5, AccountNumber = "ACC202", AccountHolder = "Ryōhei Kimura", Balance = 500.00m }
            );

            // Seed initial transactions
            modelBuilder.Entity<Transaction>().HasData(
                new Transaction { Id = 1, AccountId = 1, TransactionType = "Deposit", Amount = 500.00m, TransactionDate = DateTime.Now },
                new Transaction { Id = 2, AccountId = 1, TransactionType = "Withdrawal", Amount = 200.00m, TransactionDate = DateTime.Now },
                new Transaction { Id = 3, AccountId = 2, TransactionType = "Deposit", Amount = 1000.00m, TransactionDate = DateTime.Now },
                new Transaction { Id = 4, AccountId = 2, TransactionType = "Withdrawal", Amount = 300.00m, TransactionDate = DateTime.Now },
                new Transaction { Id = 5, AccountId = 3, TransactionType = "Deposit", Amount = 700.00m, TransactionDate = DateTime.Now },
                new Transaction { Id = 6, AccountId = 3, TransactionType = "Withdrawal", Amount = 100.00m, TransactionDate = DateTime.Now },
                new Transaction { Id = 7, AccountId = 4, TransactionType = "Deposit", Amount = 2000.00m, TransactionDate = DateTime.Now },
                new Transaction { Id = 8, AccountId = 4, TransactionType = "Withdrawal", Amount = 500.00m, TransactionDate = DateTime.Now },
                new Transaction { Id = 9, AccountId = 5, TransactionType = "Deposit", Amount = 400.00m, TransactionDate = DateTime.Now },
                new Transaction { Id = 10, AccountId = 5, TransactionType = "Withdrawal", Amount = 150.00m, TransactionDate = DateTime.Now }
            );




        }

    }
}
