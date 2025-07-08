using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myFlightPerformance.Migrations
{
    /// <inheritdoc />
    public partial class DesignUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Observations_Malfunctions_MalfunctionMalId",
                table: "Observations");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_Others_OtherId",
                table: "Observations");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_Procedures_ProcedureProId",
                table: "Observations");

            migrationBuilder.DropTable(
                name: "Malfunctions");

            migrationBuilder.DropTable(
                name: "Others");

            migrationBuilder.DropTable(
                name: "Procedures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ObservedGrades",
                table: "ObservedGrades");

            migrationBuilder.DropIndex(
                name: "IX_Observations_MalfunctionMalId",
                table: "Observations");

            migrationBuilder.DropIndex(
                name: "IX_Observations_OtherId",
                table: "Observations");

            migrationBuilder.DropIndex(
                name: "IX_Observations_ProcedureProId",
                table: "Observations");

            migrationBuilder.DropColumn(
                name: "ForeignKeyMalId",
                table: "Observations");

            migrationBuilder.DropColumn(
                name: "ForeignKeyOtherId",
                table: "Observations");

            migrationBuilder.DropColumn(
                name: "ForeignKeyProId",
                table: "Observations");

            migrationBuilder.DropColumn(
                name: "MalfunctionMalId",
                table: "Observations");

            migrationBuilder.DropColumn(
                name: "OtherId",
                table: "Observations");

            migrationBuilder.DropColumn(
                name: "ProcedureProId",
                table: "Observations");

            migrationBuilder.AddColumn<string>(
                name: "TrainingProgram",
                table: "SegDocuments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ObservedGrades",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ObservedGrades",
                table: "ObservedGrades",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EventTriggers",
                columns: table => new
                {
                    TrigId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrigCat = table.Column<string>(type: "TEXT", nullable: false),
                    TrigDes = table.Column<string>(type: "TEXT", nullable: false),
                    TrigType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTriggers", x => x.TrigId);
                });

            migrationBuilder.CreateTable(
                name: "SegDocumentJunctions",
                columns: table => new
                {
                    SegId = table.Column<int>(type: "INTEGER", nullable: false),
                    TrigId = table.Column<int>(type: "INTEGER", nullable: false),
                    FormPos = table.Column<int>(type: "INTEGER", nullable: false),
                    ObNumbers = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegDocumentJunctions", x => new { x.SegId, x.TrigId });
                    table.ForeignKey(
                        name: "FK_SegDocumentJunctions_EventTriggers_TrigId",
                        column: x => x.TrigId,
                        principalTable: "EventTriggers",
                        principalColumn: "TrigId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SegDocumentJunctions_SegDocuments_SegId",
                        column: x => x.SegId,
                        principalTable: "SegDocuments",
                        principalColumn: "SegId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObservedGrades_EventId",
                table: "ObservedGrades",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservedGrades_ObsId",
                table: "ObservedGrades",
                column: "ObsId");

            migrationBuilder.CreateIndex(
                name: "IX_SegDocumentJunctions_TrigId",
                table: "SegDocumentJunctions",
                column: "TrigId");

            migrationBuilder.AddForeignKey(
                name: "FK_ObservedGrades_Observations_ObsId",
                table: "ObservedGrades",
                column: "ObsId",
                principalTable: "Observations",
                principalColumn: "ObsId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ObservedGrades_ObservedEvents_EventId",
                table: "ObservedGrades",
                column: "EventId",
                principalTable: "ObservedEvents",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObservedGrades_Observations_ObsId",
                table: "ObservedGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_ObservedGrades_ObservedEvents_EventId",
                table: "ObservedGrades");

            migrationBuilder.DropTable(
                name: "SegDocumentJunctions");

            migrationBuilder.DropTable(
                name: "EventTriggers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ObservedGrades",
                table: "ObservedGrades");

            migrationBuilder.DropIndex(
                name: "IX_ObservedGrades_EventId",
                table: "ObservedGrades");

            migrationBuilder.DropIndex(
                name: "IX_ObservedGrades_ObsId",
                table: "ObservedGrades");

            migrationBuilder.DropColumn(
                name: "TrainingProgram",
                table: "SegDocuments");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ObservedGrades",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "ForeignKeyMalId",
                table: "Observations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ForeignKeyOtherId",
                table: "Observations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ForeignKeyProId",
                table: "Observations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MalfunctionMalId",
                table: "Observations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OtherId",
                table: "Observations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcedureProId",
                table: "Observations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ObservedGrades",
                table: "ObservedGrades",
                columns: new[] { "EventId", "ObsId" });

            migrationBuilder.CreateTable(
                name: "Malfunctions",
                columns: table => new
                {
                    MalId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ForeignKeySegId = table.Column<int>(type: "INTEGER", nullable: false),
                    MalCat = table.Column<string>(type: "TEXT", nullable: false),
                    MalDesc = table.Column<string>(type: "TEXT", nullable: false),
                    SegDocumentSegId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Malfunctions", x => x.MalId);
                    table.ForeignKey(
                        name: "FK_Malfunctions_SegDocuments_SegDocumentSegId",
                        column: x => x.SegDocumentSegId,
                        principalTable: "SegDocuments",
                        principalColumn: "SegId");
                });

            migrationBuilder.CreateTable(
                name: "Others",
                columns: table => new
                {
                    OtherId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ForeignKeySegId = table.Column<int>(type: "INTEGER", nullable: false),
                    OtherCat = table.Column<string>(type: "TEXT", nullable: false),
                    OtherDesc = table.Column<string>(type: "TEXT", nullable: false),
                    SegDocumentSegId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Others", x => x.OtherId);
                    table.ForeignKey(
                        name: "FK_Others_SegDocuments_SegDocumentSegId",
                        column: x => x.SegDocumentSegId,
                        principalTable: "SegDocuments",
                        principalColumn: "SegId");
                });

            migrationBuilder.CreateTable(
                name: "Procedures",
                columns: table => new
                {
                    ProId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ForeignKeySegId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProCat = table.Column<string>(type: "TEXT", nullable: false),
                    ProDesc = table.Column<string>(type: "TEXT", nullable: false),
                    SegDocumentSegId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedures", x => x.ProId);
                    table.ForeignKey(
                        name: "FK_Procedures_SegDocuments_SegDocumentSegId",
                        column: x => x.SegDocumentSegId,
                        principalTable: "SegDocuments",
                        principalColumn: "SegId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Observations_MalfunctionMalId",
                table: "Observations",
                column: "MalfunctionMalId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_OtherId",
                table: "Observations",
                column: "OtherId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_ProcedureProId",
                table: "Observations",
                column: "ProcedureProId");

            migrationBuilder.CreateIndex(
                name: "IX_Malfunctions_SegDocumentSegId",
                table: "Malfunctions",
                column: "SegDocumentSegId");

            migrationBuilder.CreateIndex(
                name: "IX_Others_SegDocumentSegId",
                table: "Others",
                column: "SegDocumentSegId");

            migrationBuilder.CreateIndex(
                name: "IX_Procedures_SegDocumentSegId",
                table: "Procedures",
                column: "SegDocumentSegId");

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_Malfunctions_MalfunctionMalId",
                table: "Observations",
                column: "MalfunctionMalId",
                principalTable: "Malfunctions",
                principalColumn: "MalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_Others_OtherId",
                table: "Observations",
                column: "OtherId",
                principalTable: "Others",
                principalColumn: "OtherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_Procedures_ProcedureProId",
                table: "Observations",
                column: "ProcedureProId",
                principalTable: "Procedures",
                principalColumn: "ProId");
        }
    }
}
