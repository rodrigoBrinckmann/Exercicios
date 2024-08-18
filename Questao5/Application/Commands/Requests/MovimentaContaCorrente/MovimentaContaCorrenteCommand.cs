using MediatR;
using Questao5.Domain.Entities;

namespace Questao5.Application.Commands.Requests.MovimentaContaCorrente
{
    public class MovimentaContaCorrenteCommand:IRequest<Unit>
    {
        public string ContaId { get; set; }
        public string Valor { get; set; }
        public char TipoMovimento { get; set; }
        public string HoraMovimentação { get; set; }
        public Guid IdMovimento { get; set; } = Guid.NewGuid();
    }
}

