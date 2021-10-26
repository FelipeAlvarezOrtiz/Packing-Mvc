using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Packing.Core.Productos;
using Packing.Persistencia;

namespace Packing.Negocio.Presentaciones
{
    public class ListaPresentaciones
    {
        public record Query : IRequest<List<Presentacion>> { }

        public class Handler : IRequestHandler<Query, List<Presentacion>>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<List<Presentacion>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Presentaciones.ToListAsync(cancellationToken: cancellationToken);
            }
        }
    }
}
