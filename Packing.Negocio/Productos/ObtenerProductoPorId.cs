using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Packing.Core.Productos;
using Packing.Persistencia;

namespace Packing.Negocio.Productos
{
    public class ObtenerProductoPorId
    {
        public record Command : IRequest<Producto>
        {
            public int IdProducto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Producto>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Producto> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _context.Productos.FindAsync(request.IdProducto);
            }
        }
    }
}
