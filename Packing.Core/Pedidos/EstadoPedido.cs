using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Packing.Core.Pedidos
{
    public class EstadoPedido
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEstadoPedido { get; set; }
        public string NombreEstado { get; set; }
    }
}
