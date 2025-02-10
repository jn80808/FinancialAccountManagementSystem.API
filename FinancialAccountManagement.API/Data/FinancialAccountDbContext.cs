using FinancialAccountManagement.API.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace FinancialAccountManagement.API.Data
{
    public class FinancialAccountDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("your_connection_string_here");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the seed method from the separate file
            SeedData.Seed(modelBuilder);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId);
        }

    }
}
