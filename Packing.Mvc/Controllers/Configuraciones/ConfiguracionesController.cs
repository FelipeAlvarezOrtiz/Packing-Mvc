using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Packing.Mvc.Data;
using Packing.Mvc.Models.Configuraciones;

namespace Packing.Mvc.Controllers.Configuraciones
{
    [Authorize(Roles = "Administrador")]
    public class ConfiguracionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConfiguracionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Configuracion"] = await _context.Configuraciones.FirstOrDefaultAsync();
            return View();
        }

        [HttpPost("ActualizarConfig"),Authorize(Roles = "Administrador")]
        public async Task<ActionResult> ActualizarConfiguraciones([FromBody] ConfiguracionDto request)
        {
            var config = await _context.Configuraciones.FirstOrDefaultAsync();
            config.CorreoDestino = request.correoDestino;
            config.TelefonoDestino = request.telefonoDestino;
            config.NombreEmpresaCentral = request.nombreEmpresa;
            config.NombreGerentaOperaciones = request.nombreGerenta;
            _context.Configuraciones.Update(config);
            return await _context.SaveChangesAsync() > 0
                ? Ok()
                : BadRequest("Ha ocurrido un error al guardar las configuraciones");
        }
    }
}
