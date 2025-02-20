using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PairProgress.Backend.Migrations
{
    /// <inheritdoc />
    public partial class contributionsGoalId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Contributions_GoalId",
                table: "Contributions",
                column: "GoalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_Goals_GoalId",
                table: "Contributions",
                column: "GoalId",
                principalTable: "Goals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_Goals_GoalId",
                table: "Contributions");

            migrationBuilder.DropIndex(
                name: "IX_Contributions_GoalId",
                table: "Contributions");
        }
    }
}
