using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Packing.Core.Productos
{
    public class Producto
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProducto { get; set; }
        [MinLength(5, ErrorMessage = "No cumple con la cantidad mínima de carácteres"), MaxLength(50, ErrorMessage = "Cantidad de carácteres máximos excedidos"), Required(ErrorMessage = "El dato es requerido.")]
        public string NombreProducto { get; set; }
        public string RutaImagen { get; set; }
        public Presentacion Presentacion { get; set; }
        public Formato Formato { get; set; }
        public GrupoProducto Grupo { get; set; }
        public string NombreParaBusqueda { get; set; }
        public bool AfectoVencimiento { get; set; }
        public int MinDiaVencimiento { get; set; }
        public int MaxDiaVencimiento { get; set; }
    }
}
