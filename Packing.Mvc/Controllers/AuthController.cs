using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Packing.Mvc.Models;

namespace Packing.Mvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IConfiguration config)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _config = config;
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
        
    }
}
