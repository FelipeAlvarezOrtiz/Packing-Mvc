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

        public IActionResult CrearUsuario()
        {
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
    }
}
