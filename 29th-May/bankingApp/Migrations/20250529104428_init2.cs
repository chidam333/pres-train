using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bankingApp.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId1",
                table: "Transactions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId1",
                table: "Transactions",
                column: "AccountId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_AccountId1",
                table: "Transactions",
                column: "AccountId1",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_AccountId1",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AccountId1",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AccountId1",
                table: "Transactions");
        }
    }
}
