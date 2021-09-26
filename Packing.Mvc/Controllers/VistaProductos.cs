using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Packing.Mvc.Data;

namespace Packing.Mvc.Controllers
{
    public class VistaProductos : Controller
    {
        private readonly ApplicationDbContext _context;

        public VistaProductos(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles="Administrador")]
        public IActionResult EditarProducto(int idProducto)
        {
            ViewData["idProducto"] = idProducto.ToString();
            return View();
        }

        [HttpPost,Authorize(Roles="Administrador")]
        public async Task<IActionResult> ObtenerProductos()
        {
            return Ok();
        }

        [Authorize]
        public async Task<IActionResult> VerProducto(int idProducto)
        {
            if (idProducto == 0) return Redirect("/Home");
            ViewData["Producto"] = await _context.Productos.Where(x => x.IdProducto == idProducto).Include(x => x.Grupo)
                                    .Include(x=> x.Formato).Include(x=>x.Presentacion).FirstOrDefaultAsync();
            return View();
        }

    }
}
