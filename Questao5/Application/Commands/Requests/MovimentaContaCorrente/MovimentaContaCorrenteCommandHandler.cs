using Dapper;
using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Database;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Application.Commands.Requests.MovimentaContaCorrente
{
    public class MovimentaContaCorrenteCommandHandler : IRequestHandler<MovimentaContaCorrenteCommand, Unit>
    {
        private readonly IRepositorio _repositorio;
        public MovimentaContaCorrenteCommandHandler(IRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Unit> Handle(MovimentaContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            await _repositorio.InsertMovimentoContaCorrente(request);
            return Unit.Value;
        }
    }
}
