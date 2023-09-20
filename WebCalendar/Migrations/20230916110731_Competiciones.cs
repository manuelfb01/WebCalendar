using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCalendar.Migrations
{
    /// <inheritdoc />
    public partial class Competiciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Competiciones",
                columns: table => new
                {
                    ID_Competicion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompeticion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Jor_Rep_Enfrentamiento = table.Column<int>(type: "int", nullable: false),
                    Alternar_Local_Vuelta = table.Column<bool>(type: "bit", nullable: false),
                    Num_Jor_Loc = table.Column<int>(type: "int", nullable: false),
                    ID_Usuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competiciones", x => x.ID_Competicion);
                    table.ForeignKey(
                        name: "FK_Competiciones_Usuarios_ID_Usuario",
                        column: x => x.ID_Usuario,
                        principalTable: "Usuarios",
                        principalColumn: "ID_Usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Competiciones_ID_Usuario",
                table: "Competiciones",
                column: "ID_Usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Competiciones");
        }
    }
}
