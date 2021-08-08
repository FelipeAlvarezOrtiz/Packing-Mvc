using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Packing.Mvc.Models.Empresas;

namespace Packing.Mvc.Models
{
    public class AppUser : IdentityUser
    {
        [MaxLength(120)]
        public string Direccion { get; set; }
        [Display(Name = "Nombre de la empresa")]
        public Empresa Empresa { get; set; }
    }
}
