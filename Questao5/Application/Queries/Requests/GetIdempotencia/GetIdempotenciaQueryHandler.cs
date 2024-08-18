using Dapper;
using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Application.Queries.Requests.GetContaCorrente;
using Questao5.Application.Queries.Requests.GetSaldoContaCorrente;
using Questao5.Application.Queries.Responses.GetContaCorrenteViewModel;
using Questao5.Application.Queries.Responses.GetSaldoContaCorrenteViewModel;
using Questao5.Infrastructure.Database;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Application.Queries.Requests.GetIdempotencia
{
    public class GetIdempotenciaQueryHandler: IRequestHandler<GetIdempotenciaQuery, string>
    {
        private readonly IRepositorio _repositorio;
        public GetIdempotenciaQueryHandler(IRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<string> Handle(GetIdempotenciaQuery request, CancellationToken cancellationToken)
        {
            return await _repositorio.GetIdempotencia(request);            
        }
    }
}
