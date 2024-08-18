using Dapper;
using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;
using Questao5.Application.Queries.Responses.GetContaCorrenteViewModel;
using Questao5.Infrastructure.Database;

namespace Questao5.Application.Queries.Requests.GetContaCorrente
{
    public class GetContaCorrenteQueryHandler : IRequestHandler<GetContaCorrenteQuery, GetContaCorrenteViewModel>
    {        
        private readonly IRepositorio _repositorio;
        public GetContaCorrenteQueryHandler(IRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<GetContaCorrenteViewModel> Handle(GetContaCorrenteQuery request, CancellationToken cancellationToken)
        {
            return await _repositorio.GetContaCorrente(request);            
        }
    }
}
