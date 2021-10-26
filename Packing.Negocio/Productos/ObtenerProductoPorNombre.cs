using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Packing.Core.Productos;
using Packing.Persistencia;

namespace Packing.Negocio.Productos
{
    public class ObtenerProductoPorNombre
    {
        public record Command : IRequest<Producto>
        {
            public string NombreProducto { get; set; }
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
                return await _context.Productos.Where(producto => producto.NombreParaBusqueda.Equals(request.NombreProducto))
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }
    }
}
