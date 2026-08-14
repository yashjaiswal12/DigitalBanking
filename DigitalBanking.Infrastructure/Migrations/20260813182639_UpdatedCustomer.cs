using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalBanking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FailedLoginAttempts",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastFailedLoginAt",
                table: "Customer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginAt",
                table: "Customer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TokenVersion",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Role",
                table: "Customer",
                column: "Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customer_Role",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "FailedLoginAttempts",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LastFailedLoginAt",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LastLoginAt",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "TokenVersion",
                table: "Customer");
        }
    }
}
