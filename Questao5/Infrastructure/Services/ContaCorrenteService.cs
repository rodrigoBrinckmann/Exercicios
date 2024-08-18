using MediatR;
using Questao5.Application.Commands.Requests.MovimentaContaCorrente;
using Questao5.Application.Queries.Requests.GetContaCorrente;
using Questao5.Application.Queries.Requests.GetSaldoContaCorrente;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Services.Calculation;
using Questao5.Infrastructure.Services.Validators;
using System.Globalization;

namespace Questao5.Infrastructure.Services
{
    public interface IContaCorrenteService
    {
        Task<ContaCorrente> BuscaContaCorrente(string numerocontaCorrente);
        Task<ContaCorrente> BuscaContaCorrenteParaMovimentacao(MovimentacaoConta numerocontaCorrente);
        Task<Saldo> AcessarSaldoContaCorrente(ContaCorrente contaCorrente);
        Task<string> MovimentaContaCorrente(MovimentacaoConta dadosMovimentacao);
    }

    public class ContaCorrenteService : IContaCorrenteService
    {
        private readonly IMediator _mediator;

        public ContaCorrenteService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ContaCorrente> BuscaContaCorrente(string numerocontaCorrente)
        {
            var query = new GetContaCorrenteQuery(numerocontaCorrente);
            var contaCorrente = await _mediator.Send(query);

            ValidaDados.ValidaContaCorrente(contaCorrente, "saldo");

            return new ContaCorrente(contaCorrente.Nome, contaCorrente.IdContaCorrente, numerocontaCorrente);
        }

        public async Task<ContaCorrente> BuscaContaCorrenteParaMovimentacao(MovimentacaoConta dadosMovimentacao)
        {
            var query = new GetContaCorrenteQuery(dadosMovimentacao.ContaCorrente);
            var contaCorrente = await _mediator.Send(query);
            ValidaDados.ValidaContaCorrente(contaCorrente, "movimentação");
            ValidaDados.ValidaMovimentacao(dadosMovimentacao);
            dadosMovimentacao.IdContaCorrente = contaCorrente.IdContaCorrente;
            return new ContaCorrente(contaCorrente.Nome, contaCorrente.IdContaCorrente, dadosMovimentacao.ContaCorrente);
        }

        public async Task<Saldo> AcessarSaldoContaCorrente(ContaCorrente contaCorrente)
        {
            var contaSaldo = new GetSaldoContaCorrenteQuery(contaCorrente.IdContaCorrente);
            var ocorrenciasConta = await _mediator.Send(contaSaldo);

            var calculoSaldo = CalculosSaldo.CalculaSaldoContaCorrente(ocorrenciasConta);
            var saldo = new Saldo { ContaCorrenteNumero = contaCorrente.NumeroContaCorrente, TitularContaCorente = contaCorrente.Nome, HoraDaConsulta = DateTime.Now, SaldoAtual = calculoSaldo.ToString("0.00", CultureInfo.InvariantCulture) };
            return saldo;
        }        

        public async Task<string> MovimentaContaCorrente(MovimentacaoConta dadosMovimentacao)
        {
            var tipoMovimentacao = dadosMovimentacao.TipoMovimento == EnumTipoMovimento.Débito ? 'D' : 'C';
            var movimentacao = new MovimentaContaCorrenteCommand() { ContaId = dadosMovimentacao.IdContaCorrente, TipoMovimento = tipoMovimentacao, Valor = dadosMovimentacao.Valor.ToString("0.00", CultureInfo.InvariantCulture), HoraMovimentação = DateTime.Now.ToString("dd/MM/yyyy") };
            await _mediator.Send(movimentacao);
            return movimentacao.IdMovimento.ToString();
        }        
    }
}
