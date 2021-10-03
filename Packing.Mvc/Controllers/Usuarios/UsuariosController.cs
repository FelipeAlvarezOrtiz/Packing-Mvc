using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Packing.Mvc.Data;
using Packing.Mvc.Models.Usuarios;

namespace Packing.Mvc.Controllers.Usuarios
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("ObtenerUsuarios"),Authorize(Roles = "Administrador")]
        public async Task<ActionResult<List<UsuarioInterno>>> ObtenerUsuariosDeEmpresa()
        {
            return await _context.UsuariosInternos.Where(user => user.UsuarioActivo).Include(user => user.Cargo)
                .ToListAsync();
        }

        [HttpPost(""), Authorize(Roles = "Administrador")]
        public async Task<ActionResult> InsertarUsuario([FromBody] NuevoUsuarioInterno request)
        {
            var usuarioInterno = await _context.UsuariosInternos.Where(user => user.RutUsuario.Equals(request.rutUsuario))
                .FirstOrDefaultAsync();
            if (usuarioInterno is not null) return BadRequest("Ese usuario ya está registrado en el sistema.");
            var cargo = await _context.CargosInternos.Where(cargo => cargo.IdCargo == request.idCargoInterno).FirstOrDefaultAsync();
            if (cargo is null) return BadRequest("Ese cargo no existe.");
            usuarioInterno = new UsuarioInterno()
            {
                RutUsuario = request.rutUsuario,
                UsuarioActivo = true,
                Cargo = cargo,
                CorreoUsuario = request.correoUsuario,
                NombreUsuario = request.nombreUsuario,
                NumeroTelefono = request.numeroTelefono
            };
            await _context.UsuariosInternos.AddAsync(usuarioInterno);
            return await _context.SaveChangesAsync() > 0
                ? Ok(usuarioInterno.IdUsuario)
                : BadRequest("Ha ocurrido un error al guardar el usuario");
        }

        [HttpPost("ObtenerCargosInternos"), Authorize(Roles = "Administrador")]
        public async Task<ActionResult<List<CargoInterno>>> ObtenerCargos()
        {
            return await _context.CargosInternos.ToListAsync();
        }

        [HttpPost("ActualizarUsuario"), Authorize(Roles = "Administrador")]
        public async Task<ActionResult> ActualizarUsuario([FromBody]ActualizarUsuario request)
        {
            var usuario = await _context.UsuariosInternos.Where(user => user.IdUsuario == request.idUsuario)
                .FirstOrDefaultAsync();
            if (usuario is null) return BadRequest("El usuario a actualizar no existe en sistema.");
            var cargo = await _context.CargosInternos.Where(cargo => cargo.IdCargo == request.idCargoInterno).FirstOrDefaultAsync();
            if (cargo is null) return BadRequest("Ese cargo no existe.");
            usuario.Cargo = cargo;
            usuario.RutUsuario = request.rutUsuario;
            usuario.CorreoUsuario = request.correoUsuario;
            usuario.NombreUsuario = request.nombreUsuario;
            usuario.NumeroTelefono = request.numeroTelefono;
            _context.UsuariosInternos.Update(usuario);
            return await _context.SaveChangesAsync() > 0 ? Ok() : BadRequest("No se ha podido actualizar los datos al usuario.");
        }

        [HttpPost("DeshabilitarUsuario"),Authorize(Roles = "Administrador")]
        public async Task<ActionResult> DeshabilitarUsuario([FromBody]DeshabilitarUsuario request)
        {
            var usuario = await _context.UsuariosInternos.Where(user => user.IdUsuario == request.idUsuario).Include(user => user.Cargo)
                .FirstOrDefaultAsync();
            if (usuario is null) return BadRequest("El usuario a actualizar no existe en sistema.");
            if (!usuario.UsuarioActivo) return BadRequest("El usuario ya esta deshabilitado");
            usuario.UsuarioActivo = false;
            _context.UsuariosInternos.Update(usuario);
            return await _context.SaveChangesAsync() > 0 ? Ok() : BadRequest("Ha ocurrido un error al deshabilitar al usuario");
        }

        [HttpPost("ExisteUsuario"), Authorize(Roles = "Administrador")]
        public async Task<ActionResult> ExisteUsuario([FromBody]ExisteUsuario request)
        {
            var usuario = await _context.UsuariosInternos.Where(user => user.RutUsuario.Equals(request.rutUsuario))
                .FirstOrDefaultAsync();
            return (usuario is null)
                ? Ok()
                : BadRequest("Ese rut ya está asociado a otro trabajador, intenta cambiandole el cargo.");
        }

        [Authorize(Roles="Administrador")]
        public async Task<IActionResult> CrearUsuarioInterno()
        {
            ViewData["RolesInternos"] = await _context.CargosInternos.ToListAsync();
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EditarUsuarioInterno(int idUsuario)
        {
            ViewData["CargosInternos"] = await _context.CargosInternos.ToListAsync();
            ViewData["UsuarioInterno"] = await _context.UsuariosInternos.Where(user => user.IdUsuario == idUsuario)
                .Include(user => user.Cargo).FirstOrDefaultAsync();
            return View();
        }

        [HttpPost("ActualizarUsuarioInterno"),Authorize(Roles = "Administrador")]
        public async Task<ActionResult> ActualizarUsuarioInterno([FromBody] ActualizarUsuarioInterno request)
        {
            var usuario = await _context.UsuariosInternos.Where(user => user.IdUsuario == request.idUsuario)
                .Include(user => user.Cargo).FirstOrDefaultAsync();
            if (usuario is null) return BadRequest("El usuario a actualizar no existe");
            var cargo = await _context.CargosInternos.Where(cargo => cargo.IdCargo == request.idCargoInterno).FirstOrDefaultAsync();
            if (cargo is null) return BadRequest("El cargo no existe.");
            usuario.Cargo = cargo;
            usuario.CorreoUsuario = request.correoUsuario;
            usuario.NumeroTelefono = request.numeroTelefono;
            usuario.NombreUsuario = request.nombreUsuario;
            _context.UsuariosInternos.Update(usuario);
            return await _context.SaveChangesAsync() > 0 ? Ok() : BadRequest("Ha ocurrido un error al actualizar el usuario interno.");
        }

        [HttpPost("HabilitarUsuario"), Authorize(Roles = "Administrador")]
        public async Task<ActionResult> HabilitarUsuario([FromBody] DeshabilitarUsuario request)
        {
            var usuario = await _context.UsuariosInternos.Where(user => user.IdUsuario == request.idUsuario).Include(user => user.Cargo)
                .FirstOrDefaultAsync();
            if (usuario is null) return BadRequest("El usuario a actualizar no existe en sistema.");
            if (usuario.UsuarioActivo) return BadRequest("El usuario ya esta habilitado");
            usuario.UsuarioActivo = true;
            _context.UsuariosInternos.Update(usuario);
            return await _context.SaveChangesAsync() > 0 ? Ok() : BadRequest("Ha ocurrido un error al deshabilitar al usuario");
        }

    }
}
