using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Packing.Persistencia;

namespace Packing.Negocio.Formatos
{
    public class ActualizarFormato
    {
        public record Command : IRequest
        {
            public int IdFormato { get; set; }
            public int CantidadPorFormato { get; set; }
            public string NombreFormato { get; set; }
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
                if(await _mediator.Send(new ObtenerFormato.Command{IdFormato = request.IdFormato}, cancellationToken) is null)
                    throw new Exception("No existe ese formato");
                var formato = await  _context.Formatos.FindAsync(request.IdFormato);
                
                formato.NombreFormato = request.NombreFormato;
                formato.UnidadPorFormato = request.CantidadPorFormato;

                return await _context.SaveChangesAsync(cancellationToken) > 0 ? Unit.Value : throw new Exception("Ha ocurrido un error al guardar el formato.");
            }
        }
    }
}
