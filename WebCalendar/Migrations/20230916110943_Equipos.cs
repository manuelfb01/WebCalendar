using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCalendar.Migrations
{
    /// <inheritdoc />
    public partial class Equipos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipos",
                columns: table => new
                {
                    ID_Equipo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEquipo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ID_Competicion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipos", x => x.ID_Equipo);
                    table.ForeignKey(
                        name: "FK_Equipos_Competiciones_ID_Competicion",
                        column: x => x.ID_Competicion,
                        principalTable: "Competiciones",
                        principalColumn: "ID_Competicion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipos_ID_Competicion",
                table: "Equipos",
                column: "ID_Competicion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipos");
        }
    }
}
