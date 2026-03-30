using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EntitiesRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseLogs_Exercises_ExerciseId",
                table: "ExerciseLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutTemplateExercises_Exercises_ExercisesId",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutTemplateExercises_WorkoutTemplates_WorkoutTemplatesId",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutTemplateExercises",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutTemplateExercises_WorkoutTemplatesId",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropIndex(
                name: "IX_SetLogs_ExerciseLogId",
                table: "SetLogs");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseLogs_WorkoutSessionId",
                table: "ExerciseLogs");

            migrationBuilder.DropColumn(
                name: "AverageRestTime",
                table: "WorkoutSessions");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "WorkoutSessions");

            migrationBuilder.RenameColumn(
                name: "WorkoutTemplatesId",
                table: "WorkoutTemplateExercises",
                newName: "WorkoutTemplateId");

            migrationBuilder.RenameColumn(
                name: "ExercisesId",
                table: "WorkoutTemplateExercises",
                newName: "ExerciseId");

            migrationBuilder.RenameColumn(
                name: "RestTime",
                table: "SetLogs",
                newName: "RestTimeSeconds");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "WorkoutTemplateExercises",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "WorkoutTemplateExercises",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "WorkoutTemplateExercises",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TargetReps",
                table: "WorkoutTemplateExercises",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetSets",
                table: "WorkoutTemplateExercises",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TargetWeight",
                table: "WorkoutTemplateExercises",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "WorkoutTemplateExercises",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "SetLogs",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkoutTemplateExerciseId",
                table: "ExerciseLogs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutTemplateExercises",
                table: "WorkoutTemplateExercises",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateExercises_ExerciseId",
                table: "WorkoutTemplateExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateExercises_WorkoutTemplateId_Order",
                table: "WorkoutTemplateExercises",
                columns: new[] { "WorkoutTemplateId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SetLogs_ExerciseLogId_Order",
                table: "SetLogs",
                columns: new[] { "ExerciseLogId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogs_WorkoutSessionId_Order",
                table: "ExerciseLogs",
                columns: new[] { "WorkoutSessionId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogs_WorkoutTemplateExerciseId",
                table: "ExerciseLogs",
                column: "WorkoutTemplateExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseLogs_Exercises_ExerciseId",
                table: "ExerciseLogs",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseLogs_WorkoutTemplateExercises_WorkoutTemplateExerci~",
                table: "ExerciseLogs",
                column: "WorkoutTemplateExerciseId",
                principalTable: "WorkoutTemplateExercises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutTemplateExercises_Exercises_ExerciseId",
                table: "WorkoutTemplateExercises",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutTemplateExercises_WorkoutTemplates_WorkoutTemplateId",
                table: "WorkoutTemplateExercises",
                column: "WorkoutTemplateId",
                principalTable: "WorkoutTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseLogs_Exercises_ExerciseId",
                table: "ExerciseLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseLogs_WorkoutTemplateExercises_WorkoutTemplateExerci~",
                table: "ExerciseLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutTemplateExercises_Exercises_ExerciseId",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutTemplateExercises_WorkoutTemplates_WorkoutTemplateId",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutTemplateExercises",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutTemplateExercises_ExerciseId",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutTemplateExercises_WorkoutTemplateId_Order",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropIndex(
                name: "IX_SetLogs_ExerciseLogId_Order",
                table: "SetLogs");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseLogs_WorkoutSessionId_Order",
                table: "ExerciseLogs");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseLogs_WorkoutTemplateExerciseId",
                table: "ExerciseLogs");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropColumn(
                name: "TargetReps",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropColumn(
                name: "TargetSets",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropColumn(
                name: "TargetWeight",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropColumn(
                name: "WorkoutTemplateExerciseId",
                table: "ExerciseLogs");

            migrationBuilder.RenameColumn(
                name: "WorkoutTemplateId",
                table: "WorkoutTemplateExercises",
                newName: "WorkoutTemplatesId");

            migrationBuilder.RenameColumn(
                name: "ExerciseId",
                table: "WorkoutTemplateExercises",
                newName: "ExercisesId");

            migrationBuilder.RenameColumn(
                name: "RestTimeSeconds",
                table: "SetLogs",
                newName: "RestTime");

            migrationBuilder.AddColumn<int>(
                name: "AverageRestTime",
                table: "WorkoutSessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "WorkoutSessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "SetLogs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutTemplateExercises",
                table: "WorkoutTemplateExercises",
                columns: new[] { "ExercisesId", "WorkoutTemplatesId" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateExercises_WorkoutTemplatesId",
                table: "WorkoutTemplateExercises",
                column: "WorkoutTemplatesId");

            migrationBuilder.CreateIndex(
                name: "IX_SetLogs_ExerciseLogId",
                table: "SetLogs",
                column: "ExerciseLogId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogs_WorkoutSessionId",
                table: "ExerciseLogs",
                column: "WorkoutSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseLogs_Exercises_ExerciseId",
                table: "ExerciseLogs",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
