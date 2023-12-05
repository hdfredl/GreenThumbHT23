using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenThumbHT23.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Plants",
                columns: new[] { "PlantId", "PlantDescription", "PlantName" },
                values: new object[] { 8, "Test flower", "Test Rose" });

            migrationBuilder.InsertData(
                table: "Instructions",
                columns: new[] { "InstructionId", "InstructionDescription", "InstructionName", "PlantId" },
                values: new object[] { 8, " Pour water every 2nd minute", "pour water to plant", 8 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Plants",
                keyColumn: "PlantId",
                keyValue: 8);
        }
    }
}
