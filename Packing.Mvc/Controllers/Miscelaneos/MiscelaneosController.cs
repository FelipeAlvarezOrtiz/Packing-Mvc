using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Packing.Mvc.Data;
using Packing.Mvc.Models.Empresas;

namespace Packing.Mvc.Controllers.Miscelaneos
{
    [Authorize(Roles = "Administrador")]
    public class MiscelaneosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MiscelaneosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var grupos = await _context.Grupos.ToListAsync();
            var formatos = await _context.Formatos.ToListAsync();
            var presentaciones = await _context.Presentaciones.ToListAsync();
            ViewData["Grupos"] = grupos;
            ViewData["Formatos"] = formatos;
            ViewData["Presentaciones"] = presentaciones;
            return View();
        }

        public async Task<IActionResult> EditarGrupo(int idGrupo)
        {    
            var Grupo = await _context.Grupos.Where(grupo => grupo.IdGrupo == idGrupo).FirstOrDefaultAsync();
            if (Grupo is null) return Redirect(Url.Action("Index"));
            ViewData["Grupo"] = Grupo;
            return View();
        }

        public async Task<IActionResult> EditarPresentacion(int idPresentacion)
        {
            var Presentacion = await _context.Presentaciones
                .Where(presentacion => presentacion.IdPresentacion == idPresentacion).FirstOrDefaultAsync();
            if (Presentacion is null) return Redirect("Miscelaneos");
            ViewData["Presentacion"] = Presentacion;
            return View();
        }

        public async Task<IActionResult> EditarFormato(int idFormato)
        {
            var Formato = await _context.Formatos.Where(formato => formato.IdFormato == idFormato)
                .FirstOrDefaultAsync();
            if (Formato is null) return Redirect(Url.Action("Index"));
            ViewData["Formato"] = Formato;
            return View();
        }
        
        [HttpPost("ActualizarFormato")]
        public async Task<ActionResult> ActualizarFormato([FromBody]ActualizarFormato request)
        {
            var formato = await _context.Formatos.Where(formato => formato.IdFormato == request.idFormato)
                .FirstOrDefaultAsync();
            if (formato is null) return BadRequest("El formato no existe.");
            formato.NombreFormato = request.nombreFormato;
            formato.UnidadPorFormato = request.unidadesPorFormato;
            _context.Formatos.Update(formato);
            return await _context.SaveChangesAsync() > 0
                ? Ok()
                : BadRequest("Ha ocurrido un error al actualizar los datos");
        }

        [HttpPost("ActualizarPresentacion")]
        public async Task<ActionResult> ActualizarPresentacion([FromBody]ActualizarPresentacion request)
        {
            var presentacion = await _context.Presentaciones
                .Where(presentacion => presentacion.IdPresentacion == request.idPresentacion).FirstOrDefaultAsync();
            if (presentacion is null) return BadRequest("La presentacion solicitada no existe.");
            presentacion.NombrePresentacion = request.nombrePresentacion;
            _context.Presentaciones.Update(presentacion);
            return await _context.SaveChangesAsync() > 0 ? Ok() : BadRequest("Ha ocurrido un error al actualizar los datos.");
        }

        [HttpPost("ActualizarGrupo")]
        public async Task<ActionResult> ActualizarGrupo([FromBody]ActualizarGrupo request)
        {
            var grupo = await _context.Grupos.Where(grupo => grupo.IdGrupo == request.idGrupo).FirstOrDefaultAsync();
            if (grupo is null) return BadRequest("El grupo no existe.");
            grupo.NombreGrupo = request.nombreGrupo;
            _context.Grupos.Update(grupo);
            return await _context.SaveChangesAsync() > 0
                ? Ok()
                : BadRequest("Ha ocurrido un error al actualizar los datos.");
        }

        [HttpPost("EliminarGrupo")]
        public async Task<ActionResult> EliminarGrupo([FromBody]EliminarGrupo request)
        {
            var productoConGrupo = await _context.Productos.Include(producto => producto.Grupo)
                .Where(producto => producto.Grupo.IdGrupo == request.idGrupo).FirstOrDefaultAsync();
            if (productoConGrupo is not null)
                return BadRequest(
                    "Ese grupo est? asociado a un producto, prueba cambiando el nombre o eliminar el producto asociado");
            var grupo = await _context.Grupos.Where(grupo => grupo.IdGrupo == request.idGrupo).FirstOrDefaultAsync();
            if (grupo is null) return BadRequest("Ese grupo no existe.");
            _context.Remove(grupo);
            return await _context.SaveChangesAsync() > 0
                ? Ok()
                : BadRequest("Ha ocurrido un error al eliminar ese grupo, intenta m?s tarde.");
        }

        [HttpPost("EliminarFormato")]
        public async Task<ActionResult> EliminarFormato([FromBody] EliminarFormato request)
        {
            var productoConFormato = await _context.Productos.Include(producto => producto.Formato)
                .Where(producto => producto.Formato.IdFormato == request.idFormato).FirstOrDefaultAsync();
            if (productoConFormato is not null)
                return BadRequest(
                    "Ese formato est? asociado a un producto, prueba cambiando el nombre o eliminar el producto asociado");
            var formato = await _context.Formatos.Where(formato => formato.IdFormato == request.idFormato).FirstOrDefaultAsync();
            if (formato is null) return BadRequest("Ese formato no existe.");
            _context.Remove(formato);
            return await _context.SaveChangesAsync() > 0
                ? Ok()
                : BadRequest("Ha ocurrido un error al eliminar ese formato, intenta m?s tarde.");
        }

        [HttpPost("EliminarPresentacion")]
        public async Task<ActionResult> EliminarPresentacion([FromBody] EliminarPresentacion request)
        {
            var productoConPresentacion = await _context.Productos.Include(producto => producto.Presentacion)
                .Where(producto => producto.Presentacion.IdPresentacion == request.idPresentacion).FirstOrDefaultAsync();
            if (productoConPresentacion is not null)
                return BadRequest(
                    "Esa presentaci?n est? asociado a un producto, prueba cambiando el nombre o eliminar el producto asociado");
            var presentacion = await _context.Presentaciones.Where(presentacion => presentacion.IdPresentacion == request.idPresentacion).FirstOrDefaultAsync();
            if (presentacion is null) return BadRequest("Esa presentaci?n no existe.");
            _context.Remove(presentacion);
            return await _context.SaveChangesAsync() > 0
                ? Ok()
                : BadRequest("Ha ocurrido un error al eliminar esa presentacion, intenta m?s tarde.");
        }
    }

}