using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Packing.Mvc.Models.Empresas;

namespace Packing.Mvc.Models.Pedidos
{
    public class Pedido
    {
        [Required,Key]
        public Guid GuidPedido { get; set; }
        [Required]
        public Empresa EmpresaMandante { get; set; }
        [Required]
        public EstadoPedido Estado { get; set; }
        public DateTime FechaPedido { get; set; }
        public DateTime FechaUltimaActualizacion { get; set; }
        [MaxLength(250, ErrorMessage = "Limite de carácteres sobrepasados.")]
        public string Observacion { get; set; }
        public List<DetallePedido> ProductosEnPedido { get; set; }
    }

    public class EstadoPedido
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEstadoPedido { get; set; }
        public string NombreEstado { get; set; }
    }
    
    public record ActualizarPedido(Guid idPedido, int idEstado);
}
