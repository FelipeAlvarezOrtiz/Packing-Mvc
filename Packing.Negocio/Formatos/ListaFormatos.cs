using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Packing.Core.Productos;
using Packing.Persistencia;

namespace Packing.Negocio.Formatos
{
    public class ListaFormatos
    {
        public record Query : IRequest<List<Formato>>{}

        public class Handler : IRequestHandler<Query, List<Formato>>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<List<Formato>> Handle(Query request,CancellationToken cancellationToken)
            {
                return await _context.Formatos.ToListAsync(cancellationToken);
            }
        }
    }
}
