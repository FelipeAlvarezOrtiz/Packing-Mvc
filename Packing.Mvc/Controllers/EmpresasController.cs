using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Packing.Mvc.Data;
using Packing.Mvc.Models;
using Packing.Mvc.Models.Empresas;

namespace Packing.Mvc.Controllers
{
    [Authorize(Roles="Administrador")]
    public class EmpresasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmpresasController(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> CrearUsuario()
        {
            var resultadoRoles = await _roleManager.Roles.Select(rol => rol.Name).ToListAsync();
            ViewData["Roles"] = resultadoRoles;
            return View();
        }
        
        public IActionResult EditarUsuario(string userId)
        {
            ViewData["guid"] = userId;
            return View();
        }

        public async Task<IActionResult> VistaRoles()
        {
            ViewData["listaRoles"] = await ObtenerRolEnSistema();
            return View();
        }

        public async Task<List<string>> ObtenerRolEnSistema()
        {
            var result = await _roleManager.Roles.ToListAsync();
            var payload = new List<string>(result.Count);
            payload.AddRange(result.Select(identityRole => identityRole.Name));
            return payload;
        }

        [HttpPost("ObtieneDatosDeEmpresa"),Authorize]
        public async Task<ActionResult> ObtieneDatosDeEmpresa([FromBody]ExisteEmpresaDto request)
        {
            var result = await _context.Empresas.Where(x => x.RutEmpresa.Equals(request.rutEmpresa)).FirstOrDefaultAsync();
            return result is null ? Ok("No existe datos") : Ok(result);
        }

        [HttpPost("CrearEmpresa"), Authorize(Roles = "Administrador")]
        public async Task<ActionResult> CrearEmpresa([FromBody]EmpresaDto request)
        {
            var resultUsuario = await _userManager.FindByEmailAsync(request.emailUsuario);
            if (resultUsuario is not null) return BadRequest("Ese correo ya esta asociado a un usuario");
            var resultEmpresa = await _context.Empresas.Where(x => x.RutEmpresa.Equals(request.rutEmpresa.Replace(".", "")))
                .FirstOrDefaultAsync();
            if (resultEmpresa is null){
                resultEmpresa = new Empresa()
                {
                    RutEmpresa = request.rutEmpresa,
                    Direccion = request.direccion,
                    NombreEmpresa = request.nombreEmpresa,
                    PersonaContacto = request.personaContacto,
                    RazonSocial = request.razonSocial
                };
                await _context.Empresas.AddAsync(resultEmpresa);
                var resultGuardado = await _context.SaveChangesAsync();
                if (resultGuardado == 0) return BadRequest("Ha ocurrido un error al procesar la solicitud");
            }
            var nuevoUsuario = new AppUser()
            {
                Empresa = resultEmpresa,
                Direccion = request.direccion,
                Email = request.emailUsuario,
                UserName = request.emailUsuario,
                PhoneNumber = request.telefono,
                PhoneNumberConfirmed = true,
                EmailConfirmed = true
            };
            var resultadoCreacion = await _userManager.CreateAsync(nuevoUsuario, "Packing01-");
            if (!resultadoCreacion.Succeeded)
                return BadRequest(
                    $"Ha ocurrido un error al intentar crear al usuario con mensaje {resultadoCreacion.Errors.ToString()}");
            var resultUsuarioConsulta = await _userManager.FindByEmailAsync(nuevoUsuario.Email);
            if (resultUsuarioConsulta is not null)
                await _userManager.AddToRoleAsync(resultUsuarioConsulta, "Cliente");
            return Ok();

        }
    }
}
