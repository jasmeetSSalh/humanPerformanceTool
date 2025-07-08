using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myFlightPerformance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNameOfObservedGradesInstructorCritiqueRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObservedGradesInstructorCritique_ObservedEvents_EventId",
                table: "ObservedGradesInstructorCritique");

            migrationBuilder.DropForeignKey(
                name: "FK_ObservedGradesInstructorCritique_SegDocuments_SegId",
                table: "ObservedGradesInstructorCritique");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ObservedGradesInstructorCritique",
                table: "ObservedGradesInstructorCritique");

            migrationBuilder.RenameTable(
                name: "ObservedGradesInstructorCritique",
                newName: "ObservedGradesInstructorCritiques");

            migrationBuilder.RenameIndex(
                name: "IX_ObservedGradesInstructorCritique_SegId",
                table: "ObservedGradesInstructorCritiques",
                newName: "IX_ObservedGradesInstructorCritiques_SegId");

            migrationBuilder.RenameIndex(
                name: "IX_ObservedGradesInstructorCritique_EventId",
                table: "ObservedGradesInstructorCritiques",
                newName: "IX_ObservedGradesInstructorCritiques_EventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ObservedGradesInstructorCritiques",
                table: "ObservedGradesInstructorCritiques",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ObservedGradesInstructorCritiques_ObservedEvents_EventId",
                table: "ObservedGradesInstructorCritiques",
                column: "EventId",
                principalTable: "ObservedEvents",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ObservedGradesInstructorCritiques_SegDocuments_SegId",
                table: "ObservedGradesInstructorCritiques",
                column: "SegId",
                principalTable: "SegDocuments",
                principalColumn: "SegId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObservedGradesInstructorCritiques_ObservedEvents_EventId",
                table: "ObservedGradesInstructorCritiques");

            migrationBuilder.DropForeignKey(
                name: "FK_ObservedGradesInstructorCritiques_SegDocuments_SegId",
                table: "ObservedGradesInstructorCritiques");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ObservedGradesInstructorCritiques",
                table: "ObservedGradesInstructorCritiques");

            migrationBuilder.RenameTable(
                name: "ObservedGradesInstructorCritiques",
                newName: "ObservedGradesInstructorCritique");

            migrationBuilder.RenameIndex(
                name: "IX_ObservedGradesInstructorCritiques_SegId",
                table: "ObservedGradesInstructorCritique",
                newName: "IX_ObservedGradesInstructorCritique_SegId");

            migrationBuilder.RenameIndex(
                name: "IX_ObservedGradesInstructorCritiques_EventId",
                table: "ObservedGradesInstructorCritique",
                newName: "IX_ObservedGradesInstructorCritique_EventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ObservedGradesInstructorCritique",
                table: "ObservedGradesInstructorCritique",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ObservedGradesInstructorCritique_ObservedEvents_EventId",
                table: "ObservedGradesInstructorCritique",
                column: "EventId",
                principalTable: "ObservedEvents",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ObservedGradesInstructorCritique_SegDocuments_SegId",
                table: "ObservedGradesInstructorCritique",
                column: "SegId",
                principalTable: "SegDocuments",
                principalColumn: "SegId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
