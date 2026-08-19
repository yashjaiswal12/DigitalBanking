using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalBanking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class transactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Accounts_DestinationAccountId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Accounts_SourceAccountId",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "Transactions");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_SourceAccountId",
                table: "Transactions",
                newName: "IX_Transactions_SourceAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_ReferenceNumber",
                table: "Transactions",
                newName: "IX_Transactions_ReferenceNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_DestinationAccountId",
                table: "Transactions",
                newName: "IX_Transactions_DestinationAccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_DestinationAccountId",
                table: "Transactions",
                column: "DestinationAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_SourceAccountId",
                table: "Transactions",
                column: "SourceAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_DestinationAccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_SourceAccountId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "Transaction");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_SourceAccountId",
                table: "Transaction",
                newName: "IX_Transaction_SourceAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_ReferenceNumber",
                table: "Transaction",
                newName: "IX_Transaction_ReferenceNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_DestinationAccountId",
                table: "Transaction",
                newName: "IX_Transaction_DestinationAccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Accounts_DestinationAccountId",
                table: "Transaction",
                column: "DestinationAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Accounts_SourceAccountId",
                table: "Transaction",
                column: "SourceAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
