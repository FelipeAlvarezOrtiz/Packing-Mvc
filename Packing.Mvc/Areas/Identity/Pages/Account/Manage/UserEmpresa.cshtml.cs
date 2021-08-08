using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Packing.Mvc.Data;
using Packing.Mvc.Models;

namespace Packing.Mvc.Areas.Identity.Pages.Account.Manage
{
    public class UserEmpresaModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        
        [Display(Name = "Nombre de la empresa")]
        public string NombreEmpresa { get; set; }
        [Display(Name = "Razón social")]
        public string RazonSocial { get; set; }
        [Display(Name = "Rut empresa (este dato no puede modificarse)")]
        public string RutEmpresa { get; set; }
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
        [Display(Name = "Persona de contacto")]
        public string PersonaContacto { get; set; }

        public UserEmpresaModel(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        private async Task LoadAsync(AppUser user)
        {
            var email = await _userManager.GetEmailAsync(user);
            var EmpresaResult = await _context.Usuarios.Where(x => x.Email.Equals(email))
                .Include(u => u.Empresa).FirstAsync();
            NombreEmpresa = EmpresaResult.Empresa.NombreEmpresa;
            RazonSocial = EmpresaResult.Empresa.RazonSocial;
            RutEmpresa = EmpresaResult.Empresa.RutEmpresa;
            Direccion = EmpresaResult.Empresa.Direccion;
            PersonaContacto = EmpresaResult.Empresa.PersonaContacto;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"No se ha podido cargar información del usuario '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }


        public async Task<IActionResult> OnPostUpdateDatosAsync()
        {
            Console.WriteLine("Cambiado");
            return RedirectToPage();
        }
    }
}
