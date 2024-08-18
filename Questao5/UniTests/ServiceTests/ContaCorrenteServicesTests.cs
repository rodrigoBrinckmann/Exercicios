using FluentAssertions;
using MediatR;
using Moq;
using Questao5.Application.Commands.Requests.MovimentaContaCorrente;
using Questao5.Application.Queries.Requests.GetContaCorrente;
using Questao5.Application.Queries.Requests.GetSaldoContaCorrente;
using Questao5.Application.Queries.Responses.GetContaCorrenteViewModel;
using Questao5.Application.Queries.Responses.GetSaldoContaCorrenteViewModel;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Services;
using Questao5.Infrastructure.Services.Calculation;
using Questao5.Infrastructure.Services.Controllers;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace Questao5.UniTests.ServiceTests
{
    public class ContaCorrenteServicesTests
    {
        private readonly Mock<IMediator> _mediatorMock = new();
        
        public ContaCorrenteServicesTests()
        {
            
        }
        
        [Fact(DisplayName = "ContaCorrenteServices - BuscaContaCorrente")]
        public async Task ContaCorrenteServices_BuscaContaCorrente()
        {
            //arrange            
            var getContaCorrenteViewModel = new GetContaCorrenteViewModel("José", true, "1521");
                        
            _mediatorMock.Setup(s => s.Send<GetContaCorrenteViewModel>(It.IsAny<GetContaCorrenteQuery>(), new CancellationToken())).ReturnsAsync(getContaCorrenteViewModel);

            var service = new ContaCorrenteService(_mediatorMock.Object);

            //act
            var result = await service.BuscaContaCorrente("2345");

            //assert
            result.Nome.Should().Be(getContaCorrenteViewModel.Nome);
            result.NumeroContaCorrente.Should().Be("2345");
        }

        [Fact(DisplayName = "ContaCorrenteServices - BuscaContaCorrenteParaMovimentacao")]
        public async Task ContaCorrenteServices_BuscaContaCorrenteParaMovimentacao()
        {
            //arrange
            MovimentacaoConta movimentacaoConta = new MovimentacaoConta() { ContaCorrente = "555", TipoMovimento = 0, Valor = 10m};
            var getContaCorrenteViewModel = new GetContaCorrenteViewModel("José", true, "1521");

            _mediatorMock.Setup(s => s.Send<GetContaCorrenteViewModel>(It.IsAny<GetContaCorrenteQuery>(), new CancellationToken())).ReturnsAsync(getContaCorrenteViewModel);

            var service = new ContaCorrenteService(_mediatorMock.Object);

            //act
            var result = await service.BuscaContaCorrenteParaMovimentacao(movimentacaoConta);

            //assert
            result.Nome.Should().Be(getContaCorrenteViewModel.Nome);
            result.NumeroContaCorrente.Should().Be("555");
            result.IdContaCorrente.Should().Be("1521");
        }

        [Fact(DisplayName = "ContaCorrenteServices - AcessarSaldoContaCorrente")]
        public async Task ContaCorrenteServices_AcessarSaldoContaCorrente()
        {
            //arrange
            ContaCorrente contaCorrente = new ContaCorrente("José","12345-12345","888");

            var listSaldo = new List<GetSaldoContaCorrenteViewModel>()
            {
                new GetSaldoContaCorrenteViewModel() { Valor = 15m, TipoMovimento = 'C'}
            };

            _mediatorMock.Setup(s => s.Send<List<GetSaldoContaCorrenteViewModel>>(It.IsAny<GetSaldoContaCorrenteQuery>(), new CancellationToken())).ReturnsAsync(listSaldo);

            var service = new ContaCorrenteService(_mediatorMock.Object);

            //act
            var result = await service.AcessarSaldoContaCorrente(contaCorrente);

            //assert
            result.ContaCorrenteNumero.Should().Be(contaCorrente.NumeroContaCorrente);
            result.SaldoAtual.Should().Be(listSaldo[0].Valor.ToString("0.00", CultureInfo.InvariantCulture));
            result.TitularContaCorente.Should().Be(contaCorrente.Nome);
        }

        [Fact(DisplayName = "ContaCorrenteServices - MovimentaContaCorrente")]
        public async Task ContaCorrenteServices_MovimentaContaCorrente()
        {
            //arrange
            MovimentacaoConta movimentacaoConta = new MovimentacaoConta() { ContaCorrente = "555", TipoMovimento = 0, Valor = 10m };


            _mediatorMock.Setup(s => s.Send<Unit>(It.IsAny<MovimentaContaCorrenteCommand>(), new CancellationToken())).ReturnsAsync(new Unit());

            var service = new ContaCorrenteService(_mediatorMock.Object);

            //act
            var result = await service.MovimentaContaCorrente(movimentacaoConta);

            //assert
            result.Should().NotBeEmpty();            
        }
    }
}
