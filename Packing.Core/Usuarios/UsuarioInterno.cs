using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Packing.Core.Usuarios
{
    public class UsuarioInterno
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }
        [Required, MinLength(5), MaxLength(50)]
        public string NombreUsuario { get; set; }
        [MaxLength(100)]
        public string NumeroTelefono { get; set; }
        [Required, MinLength(9), MaxLength(15)]
        public string RutUsuario { get; set; }
        [MaxLength(100)]
        public string CorreoUsuario { get; set; }
        public bool UsuarioActivo { get; set; }
        public CargoInterno Cargo { get; set; }
    }
}
