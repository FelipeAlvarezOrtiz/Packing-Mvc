using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Packing.Mvc.Data;
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
        public async Task<ActionResult> GuardarGrupo([FromForm] string nombreGrupo, IFormFile archivo, CancellationToken cancellationToken)
        {
            try
            {
                if (await ExisteGrupoPorNombre(nombreGrupo))
                {
                    return BadRequest("El grupo ya existe");
                }

                if (archivo.Length <= 0) return BadRequest("El archivo no puede estar vacio");

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/productos/",
                    archivo.FileName);
                if (System.IO.File.Exists(filePath))
                    return BadRequest("Ya existe un archivo guardado con ese nombre asociado a otro grupo.");
                await using (var stream = System.IO.File.Create(filePath))
                {
                    await archivo.CopyToAsync(stream, cancellationToken);
                }

                await _context.Grupos.AddAsync(new GrupoProducto()
                {
                    NombreGrupo = nombreGrupo,
                    Imagen = archivo.FileName
                }, cancellationToken);
                return await _context.SaveChangesAsync(cancellationToken) > 0
                    ? Ok()
                    : BadRequest("Ha ocurrido un error interno, intentelo más tarde o contacte al dev.");
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        private async Task<bool> ExisteGrupoPorNombre(string nombreGrupo)
        {
            var gruposExistentes = await _context.Grupos.ToListAsync();
            if (gruposExistentes.Count <= 0) return false;
            return await _context.Grupos.Where(grupo => grupo.NombreGrupo.Equals(nombreGrupo)).FirstOrDefaultAsync() is not null;
        }
        
    }
}
