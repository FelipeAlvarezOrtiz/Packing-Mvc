using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Packing.Core.Productos;
using Packing.Persistencia;

namespace Packing.Negocio.Grupos
{
    public class ObtenerGrupo
    {
        public record Command : IRequest<GrupoProducto>
        {
            public int IdGrupo { get; set; }
        }

        public class Handler : IRequestHandler<Command, GrupoProducto>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<GrupoProducto> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Grupos.FindAsync(request.IdGrupo) is null)
                    throw new Exception("No existen datos para el grupo buscado.");
                return await _context.Grupos.FindAsync(request.IdGrupo);
            }
        }
    }
}
