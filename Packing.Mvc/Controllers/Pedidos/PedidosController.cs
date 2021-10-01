using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Packing.Mvc.Data;
using Packing.Mvc.Models;
using Packing.Mvc.Models.Pedidos;
using Packing.Mvc.Servicios.Interfaces;

namespace Packing.Mvc.Controllers
{
    public class PedidosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICarroCompra _carro;
        private readonly UserManager<AppUser> _userManager;

        public PedidosController(ApplicationDbContext context, ICarroCompra carro, UserManager<AppUser> userManager)
        {
            _context = context;
            _carro = carro;
            _userManager = userManager;
        }
        
        [Authorize]
        public async Task<IActionResult> EditarPedido(Guid guidPedido)
        {
            ViewData["PedidoId"] = guidPedido.ToString();
            ViewData["Estados"] = await _context.EstadosPedidos.ToListAsync();
            var datosDelPedido = await _context.Pedidos.Where(x => x.GuidPedido.Equals(guidPedido))
                .Include(x => x.EmpresaMandante).Include(x => x.Estado)
                .Include(x => x.ProductosEnPedido).ThenInclude(x => x.ProductoInterno).FirstOrDefaultAsync();
            if (datosDelPedido is null) return Redirect("/Home/Pedidos");
            foreach (var pedido in datosDelPedido.ProductosEnPedido)
            {
                pedido.ProductoInterno =  await _context.Productos
                    .Where(x => x.IdProducto == pedido.ProductoInterno.IdProducto)
                    .Include(x => x.Formato).Include(x => x.Presentacion)
                    .Include(x => x.Grupo).FirstAsync();
            }
            ViewData["Pedido"] = datosDelPedido;
            ViewData["Empresa"] = await _context.Empresas.Where(empresa => empresa.IdEmpresa == datosDelPedido.EmpresaMandante.IdEmpresa).FirstOrDefaultAsync();
            //ViewData["Contacto"] = (from usuariosDeEmpresa in await _context.Usuarios.Include(user => user.Empresa).Where(user => user.Empresa.IdEmpresa == datosDelPedido.EmpresaMandante.IdEmpresa).ToListAsync() select Tuple.Create(usuariosDeEmpresa.PhoneNumber, usuariosDeEmpresa.Email)).ToList();
            if (!await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(User), "Administrador")) return View();
            ViewData["TelefonosContacto"] =
                (from usuariosDeEmpresa in await _context.Usuarios.Include(user => user.Empresa)
                        .Where(user => user.Empresa.IdEmpresa == datosDelPedido.EmpresaMandante.IdEmpresa)
                        .ToListAsync()
                    select usuariosDeEmpresa.PhoneNumber).ToList();
            ViewData["CorreosContacto"] =
                (from usuariosDeEmpresa in await _context.Usuarios.Include(user => user.Empresa)
                        .Where(user => user.Empresa.IdEmpresa == datosDelPedido.EmpresaMandante.IdEmpresa)
                        .ToListAsync()
                    select usuariosDeEmpresa.Email).ToList();
            return View();
        }

        [Authorize]
        public async Task<IActionResult> CrearPedido(int idGrupo)
        {
            ViewData["Productos"] = await _context.Productos.Include(x => x.Grupo).Where(x => x.Grupo.IdGrupo == idGrupo)
                                                .Include(x => x.Formato).Include(x => x.Presentacion).ToListAsync();
            ViewData["ProductosEnCarro"] = _carro.ObtenerProductosDelCarro();
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Grupos()
        {
            ViewData["GruposExitentes"] = await _context.Grupos.ToListAsync();
            return View();
        }

        [Authorize]
        public IActionResult VerCarro()
        {
            ViewData["ProductosEnCarro"] = _carro.ObtenerProductosDelCarro();
            return View();
        }

        [HttpPost("InsertarProductoAlPedido"),Authorize()]
        public async Task<ActionResult> InsertarProductoAlPedido([FromBody]AgregarPedido request)
        {
            var productoEscogido = await _context.Productos.Where(x => x.IdProducto == request.idProducto).Include(x => x.Formato)
                                    .Include(x => x.Grupo).Include(x => x.Presentacion).FirstOrDefaultAsync();
            if (productoEscogido is null) return BadRequest("El producto ingresado al pedido no existe.");
            var cantidadEnCarro = 0;
            var itemsEnCarro = _carro.ObtenerProductosDelCarro();
            foreach (var detallePedidoDto in itemsEnCarro)
            {
                if (detallePedidoDto.productoDto.IdProducto == request.idProducto)
                {
                    _carro.EliminarProductoDelCarro(detallePedidoDto);
                    cantidadEnCarro += detallePedidoDto.cantidad;
                }
            }
            var pedidoDetalle = new DetallePedidoDto(productoEscogido, request.cantidadProducto + cantidadEnCarro);
            _carro.InsertarProductoAlCarro(pedidoDetalle);
            return Ok();
        }

        [HttpPost("EliminarProductoDelCarro"),Authorize]
        public ActionResult EliminarProductoDelCarro([FromBody] EliminarProductoDelPedido request)
        {
            try
            {
                var itemsEnCarro = _carro.ObtenerProductosDelCarro();
                foreach (var detallePedidoDto in itemsEnCarro.Where(detallePedidoDto => detallePedidoDto.productoDto.IdProducto == request.idProducto))
                {
                    _carro.EliminarProductoDelCarro(detallePedidoDto);
                    return Ok("Producto eliminado correctamente");
                }
                return Ok("No se han encontrado productos que borrar");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("ExisteProductoEnCarro"), Authorize()]
        public async Task<ActionResult> ExisteProductoEnCarro([FromBody] AgregarPedido request)
        {
            if (_carro.ObtenerProductosDelCarro().Any(detallePedidoDto => detallePedidoDto.productoDto.IdProducto == request.idProducto))
            {
                return BadRequest("El producto ya existe en su carro, ¿desea actualizar la cantidad del mismo?");
            }

            return Ok();
        }

        [HttpGet("ObtenerProductosDelCarro"), Authorize]
        public ActionResult ObtenerProductosDelCarro()
        {
            return Ok(_carro.ObtenerProductosDelCarro());
        }

        [HttpPost("ObtenerPedidos"), Authorize]
        public async Task<ActionResult<List<Pedido>>> ObtenerPedidos()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var usuarioData = await _context.Usuarios.Where(x => x.Email.Equals(user.Email)).Include(x => x.Empresa).FirstOrDefaultAsync();
            if (usuarioData is null) return BadRequest("Error de usuario.");
            var resultPedidos = await _context.Pedidos
                .Where(x => x.EmpresaMandante.RutEmpresa.Equals(usuarioData.Empresa.RutEmpresa)).Include(x => x.EmpresaMandante)
                .Include(x => x.Estado).Include(x => x.ProductosEnPedido).ToListAsync();
            return Ok(resultPedidos);
        }

        [HttpPost("IngresarPedido"),Authorize]
        public async Task<ActionResult> IngresarPedido()
        {
            try
            {
                if (_carro.ObtenerProductosDelCarro().Count <= 0)
                    return BadRequest("No existen productos en su carro.");
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                var usuarioData = await _context.Usuarios.Where(x => x.Email.Equals(user.Email)).Include(x => x.Empresa)
                    .FirstOrDefaultAsync();
                if (usuarioData is null) return BadRequest("Error de usuario.");
                var estadoInicial =
                    await _context.EstadosPedidos.Where(x => x.IdEstadoPedido == 1).FirstOrDefaultAsync();
                if (estadoInicial is null)
                    return BadRequest("Error al obtener el estado de la base de datos, no existen estados.");
                var listaProductosEnPedido = new List<DetallePedido>();
                var cabeceraPedido = Guid.NewGuid();
                foreach (var detallePedidoDto in _carro.ObtenerProductosDelCarro())
                {
                    var productoInterno = await _context.Productos
                        .Where(x => x.IdProducto == detallePedidoDto.productoDto.IdProducto).Include(x => x.Grupo)
                        .Include(x => x.Presentacion).Include(x => x.Presentacion).FirstOrDefaultAsync();
                    listaProductosEnPedido.Add(new DetallePedido()
                    {
                        Cantidad = (uint)detallePedidoDto.cantidad,
                        CantidadTotales = (uint)(detallePedidoDto.productoDto.Formato.UnidadPorFormato *
                                                 detallePedidoDto.cantidad),
                        IdDetalle = Guid.NewGuid(),
                        ProductoInterno = productoInterno
                    });
                }

                var pedidoMain = new Pedido()
                {
                    EmpresaMandante = usuarioData.Empresa,
                    Estado = estadoInicial,
                    FechaPedido = DateTime.Today,
                    FechaUltimaActualizacion = DateTime.Today,
                    GuidPedido = cabeceraPedido,
                    Observacion = "",
                    ProductosEnPedido = listaProductosEnPedido
                };
                await _context.Pedidos.AddAsync(pedidoMain);
                _carro.CleanCarrito();
                return await _context.SaveChangesAsync() > 0
                    ? Ok()
                    : BadRequest(
                        "Ha ocurrido un error al ingresar el pedido, intente más tarde o contacte al administrador.");
            }
            catch (Exception error)
            {
                return BadRequest($"Ha ocurrido un error interno.{error.Message} ");
            }
        }

        [HttpPost("ActualizarPedido"),Authorize(Roles = "Administrador")]
        public async Task<ActionResult> ActualizarPedido([FromBody] ActualizarPedido request)
        {
            var datosDelPedido = await _context.Pedidos.Where(x => x.GuidPedido.Equals(request.idPedido))
                .Include(x => x.EmpresaMandante).Include(x => x.Estado)
                .Include(x => x.ProductosEnPedido).ThenInclude(x => x.ProductoInterno).FirstOrDefaultAsync();
            if (datosDelPedido is null) return BadRequest("No se ha encontrado datos del pedido");
            foreach (var pedido in datosDelPedido.ProductosEnPedido)
            {
                pedido.ProductoInterno = await _context.Productos
                    .Where(producto => producto.IdProducto == pedido.ProductoInterno.IdProducto)
                    .Include(producto => producto.Formato).Include(producto => producto.Presentacion)
                    .Include(producto => producto.Grupo).FirstAsync();
            }
            var nuevoEstado = await _context.EstadosPedidos.Where(estado => estado.IdEstadoPedido == request.idEstado).FirstOrDefaultAsync();
            datosDelPedido.Estado = nuevoEstado;
            _context.Pedidos.Update(datosDelPedido);
            return await _context.SaveChangesAsync() > 0 ? Ok() : BadRequest("Ha ocurrido un error al actualizar el pedido");
        }
        
    }
}
