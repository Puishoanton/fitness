using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetLogUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutTemplateExercises_Exercises_ExerciseId",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropColumn(
                name: "RestTimeSeconds",
                table: "SetLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutTemplateExercises_Exercises_ExerciseId",
                table: "WorkoutTemplateExercises",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutTemplateExercises_Exercises_ExerciseId",
                table: "WorkoutTemplateExercises");

            migrationBuilder.AddColumn<int>(
                name: "RestTimeSeconds",
                table: "SetLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutTemplateExercises_Exercises_ExerciseId",
                table: "WorkoutTemplateExercises",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
