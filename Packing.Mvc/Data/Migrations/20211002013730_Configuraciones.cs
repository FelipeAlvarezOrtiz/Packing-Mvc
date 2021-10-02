using Microsoft.EntityFrameworkCore.Migrations;

namespace Packing.Mvc.Data.Migrations
{
    public partial class Configuraciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RutUsuario",
                table: "UsuariosInternos",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "UsuarioActivo",
                table: "UsuariosInternos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Configuraciones",
                columns: table => new
                {
                    IdConfiguracion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorreoDestino = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelefonoDestino = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreEmpresaCentral = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreGerentaOperaciones = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuraciones", x => x.IdConfiguracion);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuraciones");

            migrationBuilder.DropColumn(
                name: "RutUsuario",
                table: "UsuariosInternos");

            migrationBuilder.DropColumn(
                name: "UsuarioActivo",
                table: "UsuariosInternos");
        }
    }
}
