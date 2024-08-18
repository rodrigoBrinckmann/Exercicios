using Dapper;
using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Application.Commands.Requests.MovimentaContaCorrente;
using Questao5.Infrastructure.Database;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Application.Commands.Requests.Idempotencia
{
    public class IdempotenciaCommandHandler :IRequestHandler<IdempotenciaCommand, Unit>
    {
        private readonly IRepositorio _repositorio;
        public IdempotenciaCommandHandler(IRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Unit> Handle(IdempotenciaCommand request, CancellationToken cancellationToken)
        {
            await _repositorio.InsertIdempotencia(request);
            return Unit.Value;
        }
    }
}
