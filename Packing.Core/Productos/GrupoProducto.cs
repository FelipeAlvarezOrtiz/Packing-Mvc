using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Packing.Core.Productos
{
    public class GrupoProducto
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdGrupo { get; set; }
        [MinLength(3, ErrorMessage = "No cumple con la cantidad mínima de carácteres"), MaxLength(50, ErrorMessage = "Cantidad de carácteres máximos excedidos"), Required(ErrorMessage = "El dato es requerido.")]
        public string NombreGrupo { get; set; }
        public string Imagen { get; set; }
    }
}
