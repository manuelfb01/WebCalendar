using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebCalendar.Migrations
{
    /// <inheritdoc />
    public partial class Restricciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TiposRestricciones",
                columns: table => new
                {
                    ID_Tipo_Restriccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo_Restriccion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposRestricciones", x => x.ID_Tipo_Restriccion);
                });

            migrationBuilder.CreateTable(
                name: "Restricciones",
                columns: table => new
                {
                    ID_Restriccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Tipo_Restriccion = table.Column<int>(type: "int", nullable: false),
                    ID_Competicion = table.Column<int>(type: "int", nullable: false),
                    ID_Jornada = table.Column<int>(type: "int", nullable: false),
                    ID_Equipo = table.Column<int>(type: "int", nullable: false),
                    ID_EquipoRival = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restricciones", x => x.ID_Restriccion);
                    table.ForeignKey(
                        name: "FK_Restricciones_Competiciones_ID_Competicion",
                        column: x => x.ID_Competicion,
                        principalTable: "Competiciones",
                        principalColumn: "ID_Competicion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Restricciones_TiposRestricciones_ID_Tipo_Restriccion",
                        column: x => x.ID_Tipo_Restriccion,
                        principalTable: "TiposRestricciones",
                        principalColumn: "ID_Tipo_Restriccion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TiposRestricciones",
                columns: new[] { "ID_Tipo_Restriccion", "Tipo_Restriccion" },
                values: new object[,]
                {
                    { 1, "Equipo tiene que ser local" },
                    { 2, "Equipo tiene que ser visitante" },
                    { 3, "Equipos se tienen que enfrentar" },
                    { 4, "Equipos NO se tienen que enfrentar" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Restricciones_ID_Competicion",
                table: "Restricciones",
                column: "ID_Competicion");

            migrationBuilder.CreateIndex(
                name: "IX_Restricciones_ID_Tipo_Restriccion_ID_Competicion_ID_Jornada_ID_Equipo_ID_EquipoRival",
                table: "Restricciones",
                columns: new[] { "ID_Tipo_Restriccion", "ID_Competicion", "ID_Jornada", "ID_Equipo", "ID_EquipoRival" },
                unique: true,
                filter: "[ID_EquipoRival] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Restricciones");

            migrationBuilder.DropTable(
                name: "TiposRestricciones");
        }
    }
}
