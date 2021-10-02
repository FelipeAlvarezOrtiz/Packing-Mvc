using Microsoft.EntityFrameworkCore.Migrations;

namespace Packing.Mvc.Data.Migrations
{
    public partial class UsuariosInternos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CargosInternos",
                columns: table => new
                {
                    IdCargo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCargo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargosInternos", x => x.IdCargo);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosInternos",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NumeroTelefono = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CorreoUsuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CargoIdCargo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosInternos", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_UsuariosInternos_CargosInternos_CargoIdCargo",
                        column: x => x.CargoIdCargo,
                        principalTable: "CargosInternos",
                        principalColumn: "IdCargo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosInternos_CargoIdCargo",
                table: "UsuariosInternos",
                column: "CargoIdCargo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuariosInternos");

            migrationBuilder.DropTable(
                name: "CargosInternos");
        }
    }
}
