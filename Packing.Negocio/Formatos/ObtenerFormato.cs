using System;
using MediatR;
using Packing.Core.Productos;
using System.Threading;
using System.Threading.Tasks;
using Packing.Persistencia;

namespace Packing.Negocio.Formatos
{
    public class ObtenerFormato
    {
        public record Command : IRequest<Formato>
        {
            public int IdFormato { get; set; }
        }

        public class Handler : IRequestHandler<Command, Formato>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Formato> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Formatos.FindAsync(request.IdFormato) is null)
                    throw new Exception("No existen datos para el formato buscado.");
                return await _context.Formatos.FindAsync(request.IdFormato);
            }
        }
    }
}
