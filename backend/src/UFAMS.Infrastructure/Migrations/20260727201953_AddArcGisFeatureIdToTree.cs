using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UFAMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddArcGisFeatureIdToTree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArcGisFeatureId",
                table: "Trees",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trees_ArcGisFeatureId",
                table: "Trees",
                column: "ArcGisFeatureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trees_ArcGisFeatureId",
                table: "Trees");

            migrationBuilder.DropColumn(
                name: "ArcGisFeatureId",
                table: "Trees");
        }
    }
}
