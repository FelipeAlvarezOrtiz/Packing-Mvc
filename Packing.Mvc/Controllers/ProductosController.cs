using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Packing.Mvc.Data;
using Packing.Mvc.Models;
using Packing.Mvc.Models.Empresas;

namespace Packing.Mvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public ProductosController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("Insertar"),Authorize(Roles="Administrador")]
        public async Task<IActionResult> GuardarProducto(string nombreProducto, int idFormato, int idGrupo, int idPresentacion)
        {
            var incFormato = await ObtenerFormato(idFormato);
            var incGrupo = await ObtenerGrupo(idGrupo);
            var incPresentacion = await ObtenerPresentacion(idPresentacion);
            await _context.Productos.AddAsync(new Producto()
            {
                Disponibilidad = true,
                Formato = incFormato,
                Grupo = incGrupo,
                NombreProducto = nombreProducto,
                Presentacion = incPresentacion,
                NombreParaBusqueda = nombreProducto +" "+incFormato.NombreFormato+ " "+incPresentacion.NombrePresentacion+" "+incGrupo.NombreGrupo,
                Imagen = null
            });
            return await _context.SaveChangesAsync() > 0 ? Ok() : BadRequest("Ha ocurrido un error al intentar guardar el producto");
        }

        private async Task<Formato> ObtenerFormato(int idFormato)
        {
            return await _context.Formatos.Where(x => x.IdFormato == idFormato).FirstAsync();
        }

        private async Task<GrupoProducto> ObtenerGrupo(int idGrupo)
        {
            return await _context.Grupos.Where(x => x.IdGrupo == idGrupo).FirstAsync();
        }

        private async Task<Presentacion> ObtenerPresentacion(int idPresentacion)
        {
            return await _context.Presentaciones.Where(x => x.IdPresentacion == idPresentacion).FirstAsync();
        }
    }
}
