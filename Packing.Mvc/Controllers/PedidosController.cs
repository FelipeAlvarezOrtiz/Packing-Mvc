using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Packing.Mvc.Data;
using Packing.Mvc.Models.Empresas;
using Packing.Mvc.Models.Pedidos;

namespace Packing.Mvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public PedidosController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("InsertarCabecera"), Authorize]
        public async Task<IActionResult> InsertarCabeceraPedido([FromForm]int idEmpresa,[FromForm]string Observacion)
        {
            var incEmpresa = await ObtenerEmpresa(idEmpresa);
            var incEstadoPedido = await ObtenerEstadoPedido(1);
            var cabecera = Guid.NewGuid();
            var CabeceraPedido = new Pedido
            {
                GuidPedido = cabecera,
                Estado = incEstadoPedido,
                EmpresaMandante = incEmpresa,
                Observacion = Observacion,
                FechaPedido = DateTime.Now,
                FechaUltimaActualizacion = DateTime.Now
            };
            await _context.Pedidos.AddAsync(CabeceraPedido);
            return await _context.SaveChangesAsync() > 0 ? Ok(cabecera.ToString()) : BadRequest("Ha ocurrido un problema");
        }

        [HttpPost("InsertarDetalle"), Authorize]
        public async Task<IActionResult> InsertarDetallePedido([FromForm] string cabeceraPedido,[FromForm] int idProducto, [FromForm] int Observacion)
        {
            //var incEmpresa = await ObtenerEmpresa(idEmpresa);
            //var incEstadoPedido = await ObtenerEstadoPedido(1);
            //var cabecera = Guid.NewGuid();
            //var CabeceraPedido = new Pedido
            //{
            //    GuidPedido = cabecera,
            //    Estado = incEstadoPedido,
            //    EmpresaMandante = incEmpresa,
            //    Observacion = Observacion,
            //    FechaPedido = DateTime.Now,
            //    FechaUltimaActualizacion = DateTime.Now
            //};
            //await _context.Pedidos.AddAsync(CabeceraPedido);
            //return await _context.SaveChangesAsync() > 0 ? Ok(cabecera.ToString()) : BadRequest("Ha ocurrido un problema");
            return Ok();
        }

        private async Task<Empresa> ObtenerEmpresa(int idEmpresa)
        {
            return await _context.Empresas.Where(emp => emp.IdEmpresa == idEmpresa).FirstAsync();
        }

        private async Task<EstadoPedido> ObtenerEstadoPedido(int idPedido)
        {
            return await _context.EstadosPedidos.Where(est => est.IdEstadoPedido == idPedido).FirstAsync();
        }
        
    }
}
