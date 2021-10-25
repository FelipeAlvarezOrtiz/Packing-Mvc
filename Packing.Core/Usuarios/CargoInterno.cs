using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Packing.Core.Usuarios
{
    public class CargoInterno
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCargo { get; set; }
        [MinLength(5), MaxLength(50)]
        public string NombreCargo { get; set; }
    }
}
