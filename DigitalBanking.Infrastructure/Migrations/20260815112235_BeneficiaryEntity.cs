using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalBanking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BeneficiaryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Customer_CustomerId",
                table: "RefreshToken");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "RefreshToken",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Beneficiaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 20, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 20, nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 20, nullable: false),
                    BeneficiaryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BeneficiaryAccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BeneficiaryBankCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeneficiaryBankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beneficiaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beneficiaries_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Beneficiaries_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_Token",
                table: "RefreshToken",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Beneficiaries_AccountId",
                table: "Beneficiaries",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Beneficiaries_CustomerId",
                table: "Beneficiaries",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Beneficiaries_CustomerId_BeneficiaryAccountNumber",
                table: "Beneficiaries",
                columns: new[] { "CustomerId", "BeneficiaryAccountNumber" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Customer_CustomerId",
                table: "RefreshToken",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Customer_CustomerId",
                table: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Beneficiaries");

            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_Token",
                table: "RefreshToken");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Customer_CustomerId",
                table: "RefreshToken",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
