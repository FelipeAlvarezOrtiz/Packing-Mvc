using System.Collections.Generic;
using Packing.Mvc.Models;
using Packing.Mvc.Models.Pedidos;

namespace Packing.Mvc.Servicios.Interfaces
{
    public interface ICarroCompra
    {
        public List<DetallePedidoDto> ObtenerProductosDelCarro();
        public void InsertarProductoAlCarro(DetallePedidoDto producto);
        public void CleanCarrito();
        public void EliminarProductoDelCarro(DetallePedidoDto producto);
        public void ActualizarCarrito(DetallePedidoDto producto);
        public bool ExisteEnCarro(Producto producto);
    }
}
