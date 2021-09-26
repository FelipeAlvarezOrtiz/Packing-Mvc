using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Packing.Mvc.Data;
using Packing.Mvc.Models;
using Packing.Mvc.Models.Dtos;
using Packing.Mvc.Models.Empresas;

namespace Packing.Mvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public ProductosController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("Insertar"),Authorize(Roles="Administrador")]
        public async Task<IActionResult> GuardarProducto(NuevoProducto request)
        {
            var incFormato = await ObtenerFormato(request.idFormato);
            var incGrupo = await ObtenerGrupo(request.idGrupo);
            var incPresentacion = await ObtenerPresentacion(request.idPresentacion);
            var searchName = request.nombreProducto + " " + incFormato.NombreFormato + " " +
                                     incPresentacion.NombrePresentacion + " " + incGrupo.NombreGrupo;
            if (await ExisteProducto(searchName)) return BadRequest("El producto ya existe");
            await _context.Productos.AddAsync(new Producto()
            {
                Disponibilidad = true,
                Formato = incFormato,
                Grupo = incGrupo,
                NombreProducto = request.nombreProducto,
                Presentacion = incPresentacion,
                NombreParaBusqueda = searchName,
                Imagen = null
            });
            return await _context.SaveChangesAsync() > 0 ? Ok() : BadRequest("Ha ocurrido un error al intentar guardar el producto");
        }

        [HttpPost("EliminarProducto"),Authorize(Roles = "Administrador")]
        public async Task<ActionResult> EliminarProducto(BorrarProducto request)
        {
            return Ok();
        }

        private async Task<Formato> ObtenerFormato(int idFormato)
        {
            return await _context.Formatos.Where(x => x.IdFormato == idFormato).FirstAsync();
        }

        private async Task<GrupoProducto> ObtenerGrupo(int idGrupo)
        {
            return await _context.Grupos.Where(x => x.IdGrupo == idGrupo).FirstAsync();
        }

        private async Task<Presentacion> ObtenerPresentacion(int idPresentacion)
        {
            return await _context.Presentaciones.Where(x => x.IdPresentacion == idPresentacion).FirstAsync();
        }
        
        private async Task<bool> ExisteProducto(string nombreProducto)
        {
            var productosExistentes = await _context.Productos.ToListAsync();
            if (productosExistentes.Count <= 0) return false;
            return await _context.Productos.Where(producto => producto.NombreParaBusqueda.Equals(nombreProducto)).FirstOrDefaultAsync() is not null;
        }
    }
}
