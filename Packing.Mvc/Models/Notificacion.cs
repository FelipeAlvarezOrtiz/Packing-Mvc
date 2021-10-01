using System;
using System.ComponentModel.DataAnnotations;

namespace Packing.Mvc.Models
{
    public class Notificacion
    {
        [Key]
        public Guid GuidNotificacion { get; set; }
        public int TipoNotificacion { get; set; }
        public string NombreNotificacion { get; set; }
        public int Criticidad { get; set; }
        public bool Notificado { get; set; }
        public string MensajeNotificacion { get; set; }
        public string FechaIngresada { get; set; }
    }

    public record NotificacionDto(int tipoNotificacion,string nombreNotificacion, int criticidad,string mensajeNotificacion);

    public record NotificacionCabecera(Guid cabecera);
}
