using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Packing.Mvc.Data.Migrations
{
    public partial class ProductosEnPedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PedidoGuidPedido",
                table: "DetallePedidos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetallePedidos_PedidoGuidPedido",
                table: "DetallePedidos",
                column: "PedidoGuidPedido");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallePedidos_Pedidos_PedidoGuidPedido",
                table: "DetallePedidos",
                column: "PedidoGuidPedido",
                principalTable: "Pedidos",
                principalColumn: "GuidPedido",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallePedidos_Pedidos_PedidoGuidPedido",
                table: "DetallePedidos");

            migrationBuilder.DropIndex(
                name: "IX_DetallePedidos_PedidoGuidPedido",
                table: "DetallePedidos");

            migrationBuilder.DropColumn(
                name: "PedidoGuidPedido",
                table: "DetallePedidos");
        }
    }
}
