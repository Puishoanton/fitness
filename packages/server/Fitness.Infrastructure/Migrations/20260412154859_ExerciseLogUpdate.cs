using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExerciseLogUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExerciseLogs_WorkoutSessionId_Order",
                table: "ExerciseLogs");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "ExerciseLogs");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogs_WorkoutSessionId",
                table: "ExerciseLogs",
                column: "WorkoutSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExerciseLogs_WorkoutSessionId",
                table: "ExerciseLogs");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ExerciseLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogs_WorkoutSessionId_Order",
                table: "ExerciseLogs",
                columns: new[] { "WorkoutSessionId", "Order" },
                unique: true);
        }
    }
}
