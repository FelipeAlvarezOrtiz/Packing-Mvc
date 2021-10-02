using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Packing.Mvc.Models.Configuraciones
{
    public class Configuraciones
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdConfiguracion { get; set; }
        public string CorreoDestino { get; set; }
        public string TelefonoDestino { get; set; }
        public string NombreEmpresaCentral { get; set; }
        public string NombreGerentaOperaciones { get; set; }
    }

    public record ConfiguracionDto(string correoDestino,string telefonoDestino,string nombreEmpresa,string nombreGerenta);
}
