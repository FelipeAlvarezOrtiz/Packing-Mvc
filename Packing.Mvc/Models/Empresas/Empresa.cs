using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Packing.Mvc.Models.Empresas
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
    }

    public record EmpresaDto(string nombreEmpresa, string razonSocial, string rutEmpresa, string direccion, string personaContacto,string nombreUsuario,int rolUsuario,string emailUsuario,string telefono);

    public record ExisteEmpresaDto(string rutEmpresa);
}
