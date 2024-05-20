﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventTicketAPI.Migrations
{
    /// <inheritdoc />
    public partial class totalticketsadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalTickets",
                table: "TicketTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalTickets",
                table: "TicketTypes");
        }
    }
}
