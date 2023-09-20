using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCalendar.Migrations
{
    /// <inheritdoc />
    public partial class Jornadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jornadas",
                columns: table => new
                {
                    ID_Jornada = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Num_Jornada = table.Column<int>(type: "int", nullable: false),
                    Num_Vuelta = table.Column<int>(type: "int", nullable: false),
                    ID_Competicion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jornadas", x => x.ID_Jornada);
                    table.ForeignKey(
                        name: "FK_Jornadas_Competiciones_ID_Competicion",
                        column: x => x.ID_Competicion,
                        principalTable: "Competiciones",
                        principalColumn: "ID_Competicion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jornadas_ID_Competicion",
                table: "Jornadas",
                column: "ID_Competicion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jornadas");
        }
    }
}
