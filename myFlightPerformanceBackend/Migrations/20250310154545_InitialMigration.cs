using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myFlightPerformance.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ObservedGrades",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    ObsId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Grade = table.Column<int>(type: "INTEGER", nullable: false),
                    PilotEmpId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservedGrades", x => new { x.EventId, x.ObsId });
                });

            migrationBuilder.CreateTable(
                name: "SegDocuments",
                columns: table => new
                {
                    SegId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SegName = table.Column<string>(type: "TEXT", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    DocName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegDocuments", x => x.SegId);
                });

            migrationBuilder.CreateTable(
                name: "Malfunctions",
                columns: table => new
                {
                    MalId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MalCat = table.Column<string>(type: "TEXT", nullable: false),
                    MalDesc = table.Column<string>(type: "TEXT", nullable: false),
                    ForeignKeySegId = table.Column<int>(type: "INTEGER", nullable: false),
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
                name: "ObservedEvents",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SegId = table.Column<int>(type: "INTEGER", nullable: false),
                    FlyingPilot = table.Column<int>(type: "INTEGER", nullable: false),
                    NonFlyingPilot = table.Column<int>(type: "INTEGER", nullable: false),
                    Observer = table.Column<int>(type: "INTEGER", nullable: false),
                    EventCreationDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservedEvents", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_ObservedEvents_SegDocuments_SegId",
                        column: x => x.SegId,
                        principalTable: "SegDocuments",
                        principalColumn: "SegId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Others",
                columns: table => new
                {
                    OtherId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OtherCat = table.Column<string>(type: "TEXT", nullable: false),
                    OtherDesc = table.Column<string>(type: "TEXT", nullable: false),
                    ForeignKeySegId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    ProCat = table.Column<string>(type: "TEXT", nullable: false),
                    ProDesc = table.Column<string>(type: "TEXT", nullable: false),
                    ForeignKeySegId = table.Column<int>(type: "INTEGER", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Observations",
                columns: table => new
                {
                    ObsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    SubCategory = table.Column<string>(type: "TEXT", nullable: false),
                    Behaviour = table.Column<string>(type: "TEXT", nullable: false),
                    ForeignKeyMalId = table.Column<int>(type: "INTEGER", nullable: true),
                    ForeignKeyProId = table.Column<int>(type: "INTEGER", nullable: true),
                    ForeignKeyOtherId = table.Column<int>(type: "INTEGER", nullable: true),
                    MalfunctionMalId = table.Column<int>(type: "INTEGER", nullable: true),
                    OtherId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProcedureProId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observations", x => x.ObsId);
                    table.ForeignKey(
                        name: "FK_Observations_Malfunctions_MalfunctionMalId",
                        column: x => x.MalfunctionMalId,
                        principalTable: "Malfunctions",
                        principalColumn: "MalId");
                    table.ForeignKey(
                        name: "FK_Observations_Others_OtherId",
                        column: x => x.OtherId,
                        principalTable: "Others",
                        principalColumn: "OtherId");
                    table.ForeignKey(
                        name: "FK_Observations_Procedures_ProcedureProId",
                        column: x => x.ProcedureProId,
                        principalTable: "Procedures",
                        principalColumn: "ProId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Malfunctions_SegDocumentSegId",
                table: "Malfunctions",
                column: "SegDocumentSegId");

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
                name: "IX_ObservedEvents_SegId",
                table: "ObservedEvents",
                column: "SegId");

            migrationBuilder.CreateIndex(
                name: "IX_Others_SegDocumentSegId",
                table: "Others",
                column: "SegDocumentSegId");

            migrationBuilder.CreateIndex(
                name: "IX_Procedures_SegDocumentSegId",
                table: "Procedures",
                column: "SegDocumentSegId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Observations");

            migrationBuilder.DropTable(
                name: "ObservedEvents");

            migrationBuilder.DropTable(
                name: "ObservedGrades");

            migrationBuilder.DropTable(
                name: "Malfunctions");

            migrationBuilder.DropTable(
                name: "Others");

            migrationBuilder.DropTable(
                name: "Procedures");

            migrationBuilder.DropTable(
                name: "SegDocuments");
        }
    }
}
