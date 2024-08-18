using MediatR;
using Questao5.Domain.Entities;
using Questao5.Application.Queries.Responses.GetContaCorrenteViewModel;

namespace Questao5.Application.Queries.Requests.GetContaCorrente
{
    public class GetContaCorrenteQuery : IRequest<GetContaCorrenteViewModel>
    {
        public string NumeroConta { get; set; }

        public GetContaCorrenteQuery(string numeroConta)
        {
            this.NumeroConta = numeroConta;
        }
    }
}
