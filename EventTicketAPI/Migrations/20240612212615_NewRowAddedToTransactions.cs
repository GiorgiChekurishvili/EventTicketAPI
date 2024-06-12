using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventTicketAPI.Migrations
{
    /// <inheritdoc />
    public partial class NewRowAddedToTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BalanceChanges",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalanceChanges",
                table: "Transactions");
        }
    }
}
