using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

    }
}
