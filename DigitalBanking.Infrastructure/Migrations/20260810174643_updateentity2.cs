using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalBanking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateentity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "RefreshToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "RefreshToken",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "RefreshToken",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RefreshToken",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "RefreshToken",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
