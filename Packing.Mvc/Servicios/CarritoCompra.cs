using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Packing.Mvc.Models;
using Packing.Mvc.Models.Pedidos;
using Packing.Mvc.Servicios.Interfaces;

namespace Packing.Mvc.Servicios
{
    public class CarritoCompra : ICarroCompra, IDisposable
    {
        private List<DetallePedidoDto> Productos = new();
        private readonly IConfiguration _config;
        private readonly SqlConnection _connection;

        public CarritoCompra(IConfiguration config)
        {
            _config = config;
            _connection = new(_config.GetConnectionString("Conexion"));
        }

        public List<DetallePedidoDto> ObtenerProductosDelCarro()
        {
            return Productos;
        }

        public void InsertarProductoAlCarro(DetallePedidoDto producto)
        {
            Productos.Add(producto);
        }

        public void CleanCarrito()
        {
            Productos.Clear();
        }

        public void EliminarProductoDelCarro(DetallePedidoDto producto)
        {
            var nuevaLista = new List<DetallePedidoDto>();
            foreach (var productoDto in Productos)
            {
                if (productoDto.productoDto.IdProducto != producto.productoDto.IdProducto)
                {
                    nuevaLista.Add(productoDto);
                }
            }

            Productos = nuevaLista;
            //Productos.Remove(producto);
        }

        public void ActualizarCarrito(DetallePedidoDto producto)
        {
            throw new System.NotImplementedException();
        }

        public bool ExisteEnCarro(Producto producto)
        {
            throw new System.NotImplementedException();
        }

        public void LimpiarCarro()
        {
            Productos.Clear();
        }

        public void Dispose()
        {
            Productos.Clear();
        }
    }

}
