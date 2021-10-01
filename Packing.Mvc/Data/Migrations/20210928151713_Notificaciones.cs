using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Packing.Mvc.Data.Migrations
{
    public partial class Notificaciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notificaciones",
                columns: table => new
                {
                    GuidNotificacion = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoNotificacion = table.Column<int>(type: "int", nullable: false),
                    NombreNotificacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Criticidad = table.Column<int>(type: "int", nullable: false),
                    Notificado = table.Column<bool>(type: "bit", nullable: false),
                    MensajeNotificacion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificaciones", x => x.GuidNotificacion);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notificaciones");
        }
    }
}
