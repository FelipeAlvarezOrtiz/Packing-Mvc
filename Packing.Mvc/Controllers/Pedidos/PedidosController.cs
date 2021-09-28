using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Packing.Mvc.Data;
using Packing.Mvc.Models.Pedidos;
using Packing.Mvc.Servicios.Interfaces;

namespace Packing.Mvc.Controllers
{
    public class PedidosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICarroCompra _carro;

        public PedidosController(ApplicationDbContext context, ICarroCompra carro)
        {
            _context = context;
            _carro = carro;
        }


        public IActionResult EditarPedido(Guid guidPedido)
        {
            ViewData["PedidoId"] = guidPedido.ToString();
            ViewData["ProductosEnCarro"] = _carro.ObtenerProductosDelCarro();
            return View();
        }

        public IActionResult CrearPedido()
        {
            return View();
        }

        public IActionResult VerCarro()
        {
            ViewData["ProductosEnCarro"] = _carro.ObtenerProductosDelCarro();
            return View();
        }

        [HttpPost("InsertarProductoAlPedido"),Authorize()]
        public async Task<ActionResult> InsertarProductoAlPedido([FromBody]AgregarPedido request)
        {
            var productoEscogido = await _context.Productos.Where(x => x.IdProducto == request.idProducto).FirstOrDefaultAsync();
            if (productoEscogido is null) return BadRequest("El producto ingresado al pedido no existe.");
            var pedidoDetalle = new DetallePedidoDto(productoEscogido, request.cantidadProducto);
            _carro.InsertarProductoAlCarro(pedidoDetalle);
            return Ok();
        }

        [HttpGet("ObtenerProductosDelCarro"), Authorize]
        public ActionResult ObtenerProductosDelCarro()
        {
            return Ok(_carro.ObtenerProductosDelCarro());
        }

    }
}
