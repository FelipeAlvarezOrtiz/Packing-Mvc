using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Packing.Mvc.Models.Empresas
{
    public class Presentacion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPresentacion { get; set; }
        [MinLength(3, ErrorMessage = "No cumple con la cantidad mínima de carácteres"), MaxLength(50, ErrorMessage = "Cantidad de carácteres máximos excedidos"), Required(ErrorMessage = "El dato es requerido.")]
        public string NombrePresentacion { get; set; }
    }

    public record ActualizarPresentacion(int idPresentacion, string nombrePresentacion);
    public record EliminarPresentacion(int idPresentacion);
}
