using FinancialAccountManagement.API.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace FinancialAccountManagement.API.Data
{
    public class FinancialAccountDbContext : DbContext
    {
        public FinancialAccountDbContext(DbContextOptions<FinancialAccountDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SeedData.Seed(modelBuilder); // Ensure this method is correctly implemented.

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId);
        }
    }
}
