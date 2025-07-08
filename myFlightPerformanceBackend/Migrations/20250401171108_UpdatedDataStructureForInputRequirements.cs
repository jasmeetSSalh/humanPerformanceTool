using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myFlightPerformance.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDataStructureForInputRequirements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observer",
                table: "ObservedEvents");

            migrationBuilder.AddColumn<string>(
                name: "FocusArea",
                table: "ObservedEvents",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ObservedGradesInstructorCritique",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SegId = table.Column<int>(type: "INTEGER", nullable: false),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    PilotEmpId = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservedGradesInstructorCritique", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObservedGradesInstructorCritique_ObservedEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "ObservedEvents",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObservedGradesInstructorCritique_SegDocuments_SegId",
                        column: x => x.SegId,
                        principalTable: "SegDocuments",
                        principalColumn: "SegId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObservedGradesInstructorCritique_EventId",
                table: "ObservedGradesInstructorCritique",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservedGradesInstructorCritique_SegId",
                table: "ObservedGradesInstructorCritique",
                column: "SegId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObservedGradesInstructorCritique");

            migrationBuilder.DropColumn(
                name: "FocusArea",
                table: "ObservedEvents");

            migrationBuilder.AddColumn<int>(
                name: "Observer",
                table: "ObservedEvents",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
