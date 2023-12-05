using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenThumbHT23.Migrations
{
    public partial class Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    PlantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlantDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.PlantId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Instructions",
                columns: table => new
                {
                    InstructionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstructionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstructionDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructions", x => x.InstructionId);
                    table.ForeignKey(
                        name: "FK_Instructions_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "PlantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Garden",
                columns: table => new
                {
                    GardenId = table.Column<int>(type: "int", nullable: false),
                    GardenName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Garden", x => x.GardenId);
                    table.ForeignKey(
                        name: "FK_Garden_Users_GardenId",
                        column: x => x.GardenId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GardenConnections",
                columns: table => new
                {
                    GardenId = table.Column<int>(type: "int", nullable: false),
                    PlantId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GardenConnections", x => new { x.GardenId, x.PlantId });
                    table.ForeignKey(
                        name: "FK_GardenConnections_Garden_GardenId",
                        column: x => x.GardenId,
                        principalTable: "Garden",
                        principalColumn: "GardenId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GardenConnections_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "PlantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GardenConnections_PlantId",
                table: "GardenConnections",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructions_PlantId",
                table: "Instructions",
                column: "PlantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GardenConnections");

            migrationBuilder.DropTable(
                name: "Instructions");

            migrationBuilder.DropTable(
                name: "Garden");

            migrationBuilder.DropTable(
                name: "Plants");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
