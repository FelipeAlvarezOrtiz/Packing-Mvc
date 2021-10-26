using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Packing.Core.Productos;
using Packing.Persistencia;

namespace Packing.Negocio.Formatos
{
    public class CrearFormato
    {
        public record Command : IRequest
        {
            public string NombreFormato { get; set; }
            public int CantidadPorFormato { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var nuevoNombre = request.NombreFormato;
                if (await _context.Formatos.Where(formato => formato.NombreFormato.Equals(nuevoNombre))
                    .FirstOrDefaultAsync(cancellationToken) is not null)
                    throw new Exception("Ese formato ya existe.");

                await _context.Formatos.AddAsync(new Formato()
                {
                    NombreFormato = nuevoNombre,
                    UnidadPorFormato = request.CantidadPorFormato
                }, cancellationToken);
                return await _context.SaveChangesAsync(cancellationToken) > 0
                    ? Unit.Value
                    : throw new Exception("Ha ocurrido un error interno, intentelo más tarde o contacte al dev.");
            }
        }
    }
}
