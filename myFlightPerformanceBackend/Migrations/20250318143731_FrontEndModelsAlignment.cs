using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myFlightPerformance.Migrations
{
    /// <inheritdoc />
    public partial class FrontEndModelsAlignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ObNumbers",
                table: "SegDocumentJunctions",
                newName: "ObsId");

            migrationBuilder.RenameColumn(
                name: "FormPos",
                table: "SegDocumentJunctions",
                newName: "Position");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Position",
                table: "SegDocumentJunctions",
                newName: "FormPos");

            migrationBuilder.RenameColumn(
                name: "ObsId",
                table: "SegDocumentJunctions",
                newName: "ObNumbers");
        }
    }
}
