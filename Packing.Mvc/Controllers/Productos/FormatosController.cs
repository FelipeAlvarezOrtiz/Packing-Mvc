using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Packing.Mvc.Data;
using Packing.Mvc.Models.Dtos;
using Packing.Mvc.Models.Empresas;

namespace Packing.Mvc.Controllers.Productos
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormatosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FormatosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("ObtenerFormatos"),Authorize(Roles = "Administrador")]
        public async Task<ActionResult<List<Formato>>> ObtenerFormatos()
        {
            return await _context.Formatos.ToListAsync();
        }

        [HttpPost("GuardarFormato"), Authorize(Roles = "Administrador")]
        public async Task<ActionResult> GuardarFormato(NuevoFormato request)
        {
            var nuevoNombre = request.nombreFormato;
            if (await ExisteFormatoPorNombre(nuevoNombre))
            {
                return BadRequest("El formato ya existe");
            }

            await _context.Formatos.AddAsync(new Formato()
            {
                NombreFormato = nuevoNombre,
                UnidadPorFormato = request.unidadesPorFormato
            });
            return await _context.SaveChangesAsync() > 0
                ? Ok()
                : BadRequest("Ha ocurrido un error interno, intentelo más tarde o contacte al dev.");
        }

        private async Task<bool> ExisteFormatoPorNombre(string nombreFormato)
        {
            var formatosExistentes = await _context.Formatos.ToListAsync();
            if (formatosExistentes.Count <= 0) return false;
            return await _context.Formatos.Where(formato => formato.NombreFormato.Equals(nombreFormato))
                .FirstOrDefaultAsync() is not null;
        }

    }
}
