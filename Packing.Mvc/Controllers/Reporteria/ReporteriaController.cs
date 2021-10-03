using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Packing.Mvc.Data;

namespace Packing.Mvc.Controllers.Reporteria
{
    public class ReporteriaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReporteriaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> ObtenerCantidadACosechar()
        {
            var listaPedidos = await _context.Pedidos.Include(pedido => pedido.Estado)
                .Where(pedido => pedido.Estado.IdEstadoPedido > 1).ToListAsync();
            
            return Ok();
        }
    }
}
