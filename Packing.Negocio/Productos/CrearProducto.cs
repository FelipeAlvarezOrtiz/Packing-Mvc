using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Packing.Core.Productos;
using Packing.Negocio.Formatos;
using Packing.Negocio.Grupos;
using Packing.Negocio.Presentaciones;
using Packing.Persistencia;

namespace Packing.Negocio.Productos
{
    public class CrearProducto
    {
        public record Command : IRequest
        {
            public string NombreProducto { get; set; }
            public string RutaImagen { get; set; }
            public int IdPresentacion { get; set; }
            public int IdFormato { get; set; }
            public int IdGrupo { get; set; }
            public bool AfectaVencimiento { get; set; }
            public int MinDiaVencimiento { get; set; }
            public int MaxDiaVencimiento { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMediator _mediator;

            public Handler(ApplicationDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var grupo = await _mediator.Send(new ObtenerGrupo.Command { IdGrupo = request.IdGrupo }, cancellationToken);
                var formato = await _mediator.Send(new ObtenerFormato.Command { IdFormato = request.IdFormato }, cancellationToken);
                var presentacion = await _mediator.Send(new ObtenerPresentacion.Command { IdPresentacion = request.IdPresentacion }, cancellationToken);
                var nombreBusqueda = request.NombreProducto + " " + formato.NombreFormato + " " +
                                     presentacion.NombrePresentacion;
                if (await _mediator.Send(new ObtenerProductoPorNombre.Command { NombreProducto = nombreBusqueda }, cancellationToken) is not null)
                    throw new Exception("El producto con esas combinaciones ya existe.");
                await _context.Productos.AddAsync(new Producto()
                {
                    NombreProducto = request.NombreProducto,
                    Formato = formato,
                    Grupo = grupo,
                    Presentacion = presentacion,
                    RutaImagen = request.RutaImagen,
                    MaxDiaVencimiento = request.MaxDiaVencimiento,
                    AfectoVencimiento = request.AfectaVencimiento,
                    MinDiaVencimiento = request.MinDiaVencimiento,
                    NombreParaBusqueda = nombreBusqueda
                },cancellationToken);

                return await _context.SaveChangesAsync(cancellationToken) > 0 ? Unit.Value : throw new Exception("Ha ocurrido un problema al guardar el producto.");
            }
        }

    }
}
