using Dapper;
using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Application.Queries.Responses.GetContaCorrenteViewModel;
using Questao5.Application.Queries.Responses.GetSaldoContaCorrenteViewModel;
using Questao5.Infrastructure.Database;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Application.Queries.Requests.GetSaldoContaCorrente
{
    public class GetSaldoContaCorrenteQueryHandler : IRequestHandler<GetSaldoContaCorrenteQuery, List<GetSaldoContaCorrenteViewModel>>
    {
        private readonly IRepositorio _repositorio;
        public GetSaldoContaCorrenteQueryHandler(IRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<List<GetSaldoContaCorrenteViewModel>> Handle(GetSaldoContaCorrenteQuery request, CancellationToken cancellationToken)
        {
            return await _repositorio.GetSaldoContaCorrente(request);            
        }        
    }
}
