using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaqAiApi.Migrations
{
    /// <inheritdoc />
    public partial class MakeFeedbackIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BotResponses_Feedbacks_FeedbackId",
                table: "BotResponses");

            migrationBuilder.AlterColumn<int>(
                name: "FeedbackId",
                table: "BotResponses",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_BotResponses_Feedbacks_FeedbackId",
                table: "BotResponses",
                column: "FeedbackId",
                principalTable: "Feedbacks",
                principalColumn: "FeedbackId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BotResponses_Feedbacks_FeedbackId",
                table: "BotResponses");

            migrationBuilder.AlterColumn<int>(
                name: "FeedbackId",
                table: "BotResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BotResponses_Feedbacks_FeedbackId",
                table: "BotResponses",
                column: "FeedbackId",
                principalTable: "Feedbacks",
                principalColumn: "FeedbackId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
