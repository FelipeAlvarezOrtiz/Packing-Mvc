using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Packing.Core.Productos;

namespace Packing.Core.Empresas
{
    public class DisponibilidadProducto
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDisponibilidad { get; set; }
        public Producto Producto { get; set; }
        [Range(0,999999999)]
        public int Cantidad { get; set; }
    }
}
