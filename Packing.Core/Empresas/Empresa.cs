using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Packing.Core.Productos;

namespace Packing.Core.Empresas
{
    public class Empresa
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEmpresa { get; set; }
        [MinLength(5, ErrorMessage = "No cumple con la cantidad mínima de carácteres"), MaxLength(50, ErrorMessage = "Cantidad de carácteres máximos excedidos"), Required(ErrorMessage = "El dato es requerido.")]
        public string NombreEmpresa { get; set; }
        [MinLength(5, ErrorMessage = "No cumple con la cantidad mínima de carácteres"), MaxLength(50, ErrorMessage = "Cantidad de carácteres máximos excedidos"), Required(ErrorMessage = "El dato es requerido.")]
        public string RazonSocial { get; set; }
        [MinLength(10, ErrorMessage = "No cumple con la cantidad mínima de carácteres"), MaxLength(13, ErrorMessage = "Cantidad de carácteres máximos excedidos"), Required(ErrorMessage = "El dato es requerido.")]
        public string RutEmpresa { get; set; }
        [MinLength(10, ErrorMessage = "No cumple con la cantidad mínima de carácteres"), MaxLength(75, ErrorMessage = "Cantidad de carácteres máximos excedidos"), Required(ErrorMessage = "El dato es requerido.")]
        public string Direccion { get; set; }
        [MinLength(10, ErrorMessage = "No cumple con la cantidad mínima de carácteres"), MaxLength(50, ErrorMessage = "Cantidad de carácteres máximos excedidos"), Required(ErrorMessage = "El dato es requerido.")]
        public string PersonaContacto { get; set; }
        public List<DisponibilidadProducto> ProductosVisibles { get; set; }
    }
}
