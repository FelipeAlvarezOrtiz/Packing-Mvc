using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Packing.Mvc.Data.Migrations
{
    public partial class Pedidos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DetallePedidos",
                columns: table => new
                {
                    IdDetalle = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductoInternoIdProducto = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<long>(type: "bigint", nullable: false),
                    CantidadTotales = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallePedidos", x => x.IdDetalle);
                    table.ForeignKey(
                        name: "FK_DetallePedidos_Productos_ProductoInternoIdProducto",
                        column: x => x.ProductoInternoIdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EstadosPedidos",
                columns: table => new
                {
                    IdEstadoPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEstado = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosPedidos", x => x.IdEstadoPedido);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    GuidPedido = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmpresaMandanteIdEmpresa = table.Column<int>(type: "int", nullable: true),
                    EstadoIdEstadoPedido = table.Column<int>(type: "int", nullable: true),
                    FechaPedido = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.GuidPedido);
                    table.ForeignKey(
                        name: "FK_Pedidos_Empresas_EmpresaMandanteIdEmpresa",
                        column: x => x.EmpresaMandanteIdEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdEmpresa",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pedidos_EstadosPedidos_EstadoIdEstadoPedido",
                        column: x => x.EstadoIdEstadoPedido,
                        principalTable: "EstadosPedidos",
                        principalColumn: "IdEstadoPedido",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallePedidos_ProductoInternoIdProducto",
                table: "DetallePedidos",
                column: "ProductoInternoIdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_EmpresaMandanteIdEmpresa",
                table: "Pedidos",
                column: "EmpresaMandanteIdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_EstadoIdEstadoPedido",
                table: "Pedidos",
                column: "EstadoIdEstadoPedido");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallePedidos");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "EstadosPedidos");
        }
    }
}
