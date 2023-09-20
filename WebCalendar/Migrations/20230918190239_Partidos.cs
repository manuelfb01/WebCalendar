using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCalendar.Migrations
{
    /// <inheritdoc />
    public partial class Partidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partidos",
                columns: table => new
                {
                    ID_Partido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Competicion = table.Column<int>(type: "int", nullable: false),
                    ID_Jornada = table.Column<int>(type: "int", nullable: false),
                    ID_EquipoLocal = table.Column<int>(type: "int", nullable: false),
                    ID_EquipoVisitante = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partidos", x => x.ID_Partido);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Partidos");
        }
    }
}
