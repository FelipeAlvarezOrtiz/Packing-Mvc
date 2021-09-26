using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Packing.Mvc.Data;
using Packing.Mvc.Models.Dtos;
using Packing.Mvc.Models.Empresas;

namespace Packing.Mvc.Controllers.Productos
{
    [Route("api/[controller]")]
    [ApiController]
    public class GruposController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GruposController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("ObtenerGrupos"),Authorize(Roles = "Administrador")]
        public async Task<ActionResult<List<GrupoProducto>>> ObtenerGrupos()
        {
            return await _context.Grupos.ToListAsync();
        }

        [HttpPost("GuardarGrupo"), Authorize(Roles = "Administrador")]
        public async Task<ActionResult> GuardarGrupo(NuevoGrupo request)
        {
            var nuevoNombre = request.NombreGrupo;
            if (await ExisteGrupoPorNombre(nuevoNombre))
            {
                return BadRequest("El grupo ya existe");
            }

            await _context.Grupos.AddAsync(new GrupoProducto()
            {
                NombreGrupo = nuevoNombre,
                Imagen = null
            });
            return await _context.SaveChangesAsync() > 0
                ? Ok()
                : BadRequest("Ha ocurrido un error interno, intentelo más tarde o contacte al dev.");
        }

        private async Task<bool> ExisteGrupoPorNombre(string nombreGrupo)
        {
            var gruposExistentes = await _context.Grupos.ToListAsync();
            if (gruposExistentes.Count <= 0) return false;
            return await _context.Grupos.Where(grupo => grupo.NombreGrupo.Equals(nombreGrupo)).FirstOrDefaultAsync() is not null;
        }
    }
}
