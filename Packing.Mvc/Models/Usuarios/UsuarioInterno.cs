using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Packing.Mvc.Models.Usuarios
{
    public class UsuarioInterno
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }
        [Required,MinLength(5),MaxLength(50)]
        public string NombreUsuario { get; set; }
        [MaxLength(100)]
        public string NumeroTelefono { get; set; }
        [Required,MinLength(9),MaxLength(15)]
        public string RutUsuario { get; set; }
        [MaxLength(100)]
        public string CorreoUsuario { get; set; }
        public bool UsuarioActivo { get; set; }
        public CargoInterno Cargo { get; set; }
    }

    public class CargoInterno
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCargo { get; set; }
        [MinLength(5),MaxLength(50)]
        public string NombreCargo { get; set; }
    }

    public record NuevoUsuarioInterno(string nombreUsuario,string rutUsuario,string numeroTelefono, string correoUsuario,int idCargoInterno);
    public record NuevoCargo(string nombreCargo);
    public record ActualizarUsuario(int idUsuario, string nombreUsuario, string rutUsuario, string numeroTelefono, string correoUsuario, int idCargoInterno);
    public record ActualizarUsuarioInterno(int idUsuario, string nombreUsuario,string numeroTelefono, string correoUsuario, int idCargoInterno);
    public record DeshabilitarUsuario(int idUsuario);
    public record ExisteUsuario(string rutUsuario);

}
