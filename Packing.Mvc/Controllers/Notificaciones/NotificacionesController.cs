using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Packing.Mvc.Data;
using Packing.Mvc.Models;

namespace Packing.Mvc.Controllers.Notificaciones
{
    public class NotificacionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("ActualizaNotificado"), Authorize]
        public async Task<ActionResult> ActualizaNotificado([FromBody]NotificacionCabecera request)
        {
            var notificacion = await _context.Notificaciones.Where(x => x.GuidNotificacion.Equals(request.cabecera))
                .FirstOrDefaultAsync();
            notificacion.Notificado = true;
            return await _context.SaveChangesAsync() > 0 ? Ok("Datos actualizados") : BadRequest("Ha ocurrido un error interno, pruebe más tarde.");
        }

        [HttpGet("ObtenerNotificacionesPendientes"), Authorize(Roles = "Administrador")]
        public async Task<ActionResult<List<Notificacion>>> ObtenerNotificacionesPendientes()
        {
            return await _context.Notificaciones.Where(x => x.Notificado == false).ToListAsync();
        }

        [HttpPost("GuardarNotificacion"), Authorize]
        public async Task<ActionResult> GuardarNotificacion([FromBody]NotificacionDto request)
        {
            await _context.Notificaciones.AddAsync(new Notificacion()
            {
                Notificado = false,
                Criticidad = request.criticidad,
                GuidNotificacion = Guid.NewGuid(),
                MensajeNotificacion = request.mensajeNotificacion,
                NombreNotificacion = request.nombreNotificacion,
                TipoNotificacion = request.tipoNotificacion,
                FechaIngresada = DateTime.Today.ToString("dd/MM/yyyy")
            });
            return await _context.SaveChangesAsync() > 0 ? Ok() : BadRequest();
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Notificaciones"] = await _context.Notificaciones.Where(x => x.Notificado == false).ToListAsync();
            return View();
        }

    }
}
