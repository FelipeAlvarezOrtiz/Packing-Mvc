using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Packing.Mvc.Data;
using Packing.Mvc.Models;
using Packing.Mvc.Models.Empresas;

namespace Packing.Mvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public AuthController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IConfiguration config,ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _config = config;
            _context = context;
        }

        [Route("Registrar")]
        [AllowAnonymous]
        public async Task<IActionResult> RegistrarInit()
        {
            await CrearRoles();
            const string administrador = "Administrador";
            const string cliente = "Cliente";
            var user = new AppUser
            {
                Email = _config.GetValue<string>("UserClient"),
                UserName = _config.GetValue<string>("UserClient"),
                Direccion = "Vicuña"
            };
            var result = await _userManager.CreateAsync(user, "Felipe2-");
            user = await _userManager.FindByEmailAsync(_config.GetValue<string>("UserClient"));
            if (user is not null)
            {
                await _userManager.AddToRoleAsync(user, cliente);
            }
            return BadRequest();
        }

        private async Task CrearRoles()
        {
            string[] roles = {"Administrador","Cliente"};
            foreach (var rol in roles)
            {
                if (! await _roleManager.RoleExistsAsync(rol))
                {
                    await _roleManager.CreateAsync(new IdentityRole(rol));
                }
            }
        }

        [HttpPost("Test"),Authorize(Roles="Administrador")]
        public async Task<IActionResult> Prueba([FromForm]string test,[FromForm]string test2)
        {
            Console.WriteLine($"Esto e es una {test}");
            await _context.Usuarios.ToListAsync();
            return Ok();
        }

        [HttpPost("CrearRol"), Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CrearRole([FromForm]string nuevoRol)
        {
            if (await _roleManager.RoleExistsAsync(nuevoRol)) return BadRequest("El rol ya existe");
            await _roleManager.CreateAsync(new IdentityRole(nuevoRol));
            return Ok("Creado exitosamente");
        }
        

        //CREAR UN ENDPOINT EXCLUSIVO PARA EMPRESAS
        [HttpGet("ObtenerRoles"), Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ObtenerRolParaAdmin()
        {
            var result = await _roleManager.Roles.ToListAsync();
            var payload = new List<string>(result.Count);
            payload.AddRange(result.Select(identityRole => identityRole.Name));
            
            return payload.Count > 0 ? Ok(payload) : NotFound("No se han encontrado datos");
        }
    }
}
