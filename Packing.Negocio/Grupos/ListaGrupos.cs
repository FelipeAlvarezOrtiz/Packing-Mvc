using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Packing.Core.Productos;
using Packing.Persistencia;

namespace Packing.Negocio.Grupos
{
    public class ListaGrupos
    {
        public record Query : IRequest<List<GrupoProducto>> { }

        public class Handler : IRequestHandler<Query, List<GrupoProducto>>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<List<GrupoProducto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Grupos.ToListAsync(cancellationToken: cancellationToken);
            }
        }
    }
}
