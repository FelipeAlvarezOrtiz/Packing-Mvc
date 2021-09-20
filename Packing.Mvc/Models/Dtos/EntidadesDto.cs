namespace Packing.Mvc.Models.Dtos
{
    public record NuevoGrupo(string NombreGrupo);
    public record NuevoProducto(string nombreProducto, int idFormato, int idGrupo, int idPresentacion);
}
