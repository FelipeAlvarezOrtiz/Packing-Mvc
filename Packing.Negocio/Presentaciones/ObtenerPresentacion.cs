using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Packing.Core.Productos;
using Packing.Persistencia;

namespace Packing.Negocio.Presentaciones
{
    public class ObtenerPresentacion
    {
        public record Command : IRequest<Presentacion>
        {
            public int IdPresentacion { get; set; }
        }

        public class Handler : IRequestHandler<Command, Presentacion>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Presentacion> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Presentaciones.FindAsync(request.IdPresentacion) is null)
                    throw new Exception("No existen datos para la presentación buscada.");
                return await _context.Presentaciones.FindAsync(request.IdPresentacion);
            }
        }
    }
}
