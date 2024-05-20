using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventTicketAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserEntityModify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpiresPasswordChange",
                table: "Users",
                newName: "ExpiresPasswordChangeDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpiresPasswordChangeDate",
                table: "Users",
                newName: "ExpiresPasswordChange");
        }
    }
}
