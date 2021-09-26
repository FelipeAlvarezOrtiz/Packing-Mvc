using System;

namespace Packing.Mvc.Models.Dtos
{
    public record NuevoGrupo(string NombreGrupo);
    public record NuevoProducto(string nombreProducto, int idFormato, int idGrupo, int idPresentacion);
    public record NuevoFormato(string nombreFormato, int unidadesPorFormato);
    public record NuevaPresentacion(string NombrePresentacion);
    public record BorrarProducto(int idProducto);
}
