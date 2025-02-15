using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinancialAccountManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AccountHolder = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "Id", "AccountHolder", "AccountNumber", "Balance" },
                values: new object[,]
                {
                    { 1, "Chiaki Kobayashi", "ACC123", 1000.00m },
                    { 2, "Rie Takahashi", "ACC456", 2000.00m },
                    { 3, "Yumiri Hanamori", "ACC789", 1500.00m },
                    { 4, "Makoto Koichi", "ACC101", 3000.00m },
                    { 5, "Ryōhei Kimura", "ACC202", 500.00m }
                });

            migrationBuilder.InsertData(
                table: "Transaction",
                columns: new[] { "Id", "AccountId", "Amount", "TransactionDate", "TransactionType" },
                values: new object[,]
                {
                    { 1, 1, 500.00m, new DateTime(2025, 2, 15, 19, 53, 46, 498, DateTimeKind.Local).AddTicks(5824), "Deposit" },
                    { 2, 1, 200.00m, new DateTime(2025, 2, 15, 19, 53, 46, 498, DateTimeKind.Local).AddTicks(5837), "Withdrawal" },
                    { 3, 2, 1000.00m, new DateTime(2025, 2, 15, 19, 53, 46, 498, DateTimeKind.Local).AddTicks(5839), "Deposit" },
                    { 4, 2, 300.00m, new DateTime(2025, 2, 15, 19, 53, 46, 498, DateTimeKind.Local).AddTicks(5841), "Withdrawal" },
                    { 5, 3, 700.00m, new DateTime(2025, 2, 15, 19, 53, 46, 498, DateTimeKind.Local).AddTicks(5843), "Deposit" },
                    { 6, 3, 100.00m, new DateTime(2025, 2, 15, 19, 53, 46, 498, DateTimeKind.Local).AddTicks(5844), "Withdrawal" },
                    { 7, 4, 2000.00m, new DateTime(2025, 2, 15, 19, 53, 46, 498, DateTimeKind.Local).AddTicks(5846), "Deposit" },
                    { 8, 4, 500.00m, new DateTime(2025, 2, 15, 19, 53, 46, 498, DateTimeKind.Local).AddTicks(5848), "Withdrawal" },
                    { 9, 5, 400.00m, new DateTime(2025, 2, 15, 19, 53, 46, 498, DateTimeKind.Local).AddTicks(5849), "Deposit" },
                    { 10, 5, 150.00m, new DateTime(2025, 2, 15, 19, 53, 46, 498, DateTimeKind.Local).AddTicks(5851), "Withdrawal" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountId",
                table: "Transaction",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
