using Microsoft.EntityFrameworkCore.Migrations;
using System.Data.Common;

#nullable disable

namespace EventTicketAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserVerificationModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
            name: "VerifiedAt",
            table: "Users",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
           name: "VerifiedAt",
           table: "Users",
           nullable: false,
           oldClrType: typeof(DateTime),
           oldType: "datetime2",
           oldNullable: true);
        }
    }
}
