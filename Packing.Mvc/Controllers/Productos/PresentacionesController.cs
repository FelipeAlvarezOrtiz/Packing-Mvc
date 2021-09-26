using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
    public class PresentacionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PresentacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("ObtenerPresentaciones"),Authorize(Roles = "Administrador")]
        public async Task<ActionResult<List<Presentacion>>> ObtenerPresentaciones()
        {
            return await _context.Presentaciones.ToListAsync();
        }

        [HttpPost("InsertarPresentacion"), Authorize(Roles = "Administrador")]
        public async Task<ActionResult> InsertarPresentacion(NuevaPresentacion request)
        {
            var nuevoNombre = request.NombrePresentacion;
            if (await ExistePresentacionPorNombre(nuevoNombre))
            {
                return BadRequest("Esa presentación ya existe");
            }

            await _context.Presentaciones.AddAsync(new Presentacion()
            {
                NombrePresentacion = nuevoNombre,
            });
            return await _context.SaveChangesAsync() > 0
                ? Ok()
                : BadRequest("Ha ocurrido un error interno, intentelo más tarde o contacte al dev.");
        }

        private async Task<bool> ExistePresentacionPorNombre(string nombrePresentacion)
        {
            var presentacionesExistentes = await _context.Presentaciones.ToListAsync();
            if (presentacionesExistentes.Count <= 0) return false;
            return await _context.Presentaciones
                .Where(presentacion => presentacion.NombrePresentacion.Equals(nombrePresentacion))
                .FirstOrDefaultAsync() is not null;
        }
    }
}
