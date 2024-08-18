using MediatR;
using Questao5.Application.Queries.Responses.GetSaldoContaCorrenteViewModel;

namespace Questao5.Application.Queries.Requests.GetSaldoContaCorrente
{
    public class GetSaldoContaCorrenteQuery :IRequest<List<GetSaldoContaCorrenteViewModel>>
    {
        public string IdConta { get; set; }
        public GetSaldoContaCorrenteQuery(string idContaCorrente)
        {
                this.IdConta = idContaCorrente;
        }
    }
}
