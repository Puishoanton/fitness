using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMuscleGroupToExercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseWorkoutTemplate_Exercises_ExercisesId",
                table: "ExerciseWorkoutTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseWorkoutTemplate_WorkoutTemplates_WorkoutTemplatesId",
                table: "ExerciseWorkoutTemplate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExerciseWorkoutTemplate",
                table: "ExerciseWorkoutTemplate");

            migrationBuilder.RenameTable(
                name: "ExerciseWorkoutTemplate",
                newName: "WorkoutTemplateExercises");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseWorkoutTemplate_WorkoutTemplatesId",
                table: "WorkoutTemplateExercises",
                newName: "IX_WorkoutTemplateExercises_WorkoutTemplatesId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "WorkoutSessions",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutTemplateExercises",
                table: "WorkoutTemplateExercises",
                columns: new[] { "ExercisesId", "WorkoutTemplatesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutTemplateExercises_Exercises_ExercisesId",
                table: "WorkoutTemplateExercises",
                column: "ExercisesId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutTemplateExercises_WorkoutTemplates_WorkoutTemplatesId",
                table: "WorkoutTemplateExercises",
                column: "WorkoutTemplatesId",
                principalTable: "WorkoutTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutTemplateExercises_Exercises_ExercisesId",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutTemplateExercises_WorkoutTemplates_WorkoutTemplatesId",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutTemplateExercises",
                table: "WorkoutTemplateExercises");

            migrationBuilder.RenameTable(
                name: "WorkoutTemplateExercises",
                newName: "ExerciseWorkoutTemplate");

            migrationBuilder.RenameIndex(
                name: "IX_WorkoutTemplateExercises_WorkoutTemplatesId",
                table: "ExerciseWorkoutTemplate",
                newName: "IX_ExerciseWorkoutTemplate_WorkoutTemplatesId");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "WorkoutSessions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExerciseWorkoutTemplate",
                table: "ExerciseWorkoutTemplate",
                columns: new[] { "ExercisesId", "WorkoutTemplatesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseWorkoutTemplate_Exercises_ExercisesId",
                table: "ExerciseWorkoutTemplate",
                column: "ExercisesId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseWorkoutTemplate_WorkoutTemplates_WorkoutTemplatesId",
                table: "ExerciseWorkoutTemplate",
                column: "WorkoutTemplatesId",
                principalTable: "WorkoutTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
