using System;
using System.ComponentModel.DataAnnotations;

namespace Packing.Mvc.Models.Pedidos
{
    public class DetallePedido
    {
        [Key,Required]
        public Guid IdDetalle { get; set; }
        public Producto ProductoInterno { get; set; }
        public uint Cantidad { get; set; }
        public uint CantidadTotales { get; set; }
    }

    public record DetallePedidoDto(Producto productoDto,int cantidad);
}
