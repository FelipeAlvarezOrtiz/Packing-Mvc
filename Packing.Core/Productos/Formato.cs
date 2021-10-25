using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Packing.Core.Productos
{
    public class Formato
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFormato { get; set; }
        [MinLength(3, ErrorMessage = "No cumple con la cantidad mínima de carácteres"), MaxLength(50, ErrorMessage = "Cantidad de carácteres máximos excedidos"), Required(ErrorMessage = "El dato es requerido.")]
        public string NombreFormato { get; set; }
        public int UnidadPorFormato { get; set; }

    }
}
