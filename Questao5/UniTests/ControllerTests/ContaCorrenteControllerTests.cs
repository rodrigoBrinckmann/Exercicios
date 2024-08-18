using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NSubstitute.ExceptionExtensions;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Services;
using Questao5.Infrastructure.Services.Controllers;
using Xunit;

namespace Questao5.UniTests.ControllerTests
{
    public class ContaCorrenteControllerTests
    {

        private readonly Mock<IContaCorrenteService> _contaCorrenteServiceMock = new();
        private readonly Mock<IIdempotencyService> _idempotencyServiceMock = new();
        public ContaCorrenteControllerTests()
        {
               
        }

        [Fact(DisplayName = "GetSaldoContaCorrente - Returns Ok with saldo")]
        public async Task GetSaldoContaCorrente_ReturnsOk()
        {
            //arange
            string conta = "12345";
            ContaCorrente contaCorrente = new ContaCorrente("José", "555222", "12345");
            Saldo saldo = new Saldo() { ContaCorrenteNumero = "12345", HoraDaConsulta = DateTime.Now, SaldoAtual = "10", TitularContaCorente = "José" };
            _contaCorrenteServiceMock.Setup(s => s.BuscaContaCorrente(conta)).ReturnsAsync(contaCorrente);
            _contaCorrenteServiceMock.Setup(s => s.AcessarSaldoContaCorrente(contaCorrente)).ReturnsAsync(saldo);

            var controller = GetController();

            //act
            var response = await controller.GetSaldoContaCorrente(conta);

            //assert
            var result = response.Should().BeOfType<OkObjectResult>().Subject;
            result.Value.Should().Be(saldo);
        }

        [Fact(DisplayName = "MovimentaçãoContaCorrente - Returns Ok with Moviment Id")]
        public async Task MovimentaçãoContaCorrente_ReturnsOk_withIdDoMovimento()        
        {
            //arange            
            ContaCorrente contaCorrente = new ContaCorrente("José", "555222", "12345");
            MovimentacaoConta movimentacaoContaCorrente = new MovimentacaoConta() { ContaCorrente = "12345", IdContaCorrente = "555444", TipoMovimento = EnumTipoMovimento.Crédito, Valor = 10m };            
            _contaCorrenteServiceMock.Setup(s => s.BuscaContaCorrenteParaMovimentacao(movimentacaoContaCorrente)).ReturnsAsync(contaCorrente);
            _idempotencyServiceMock.Setup(s => s.ValidateIdempotency("555666")).ReturnsAsync("");
            _contaCorrenteServiceMock.Setup(s => s.MovimentaContaCorrente(movimentacaoContaCorrente)).ReturnsAsync("88888");
            _idempotencyServiceMock.Setup(s => s.AddIdempotencyControl("555666", It.IsAny<MovimentacaoConta>(), "88888"));
            
            var controller = GetController();

            //act
            var response = await controller.MovimentaçãoContaCorrente(movimentacaoContaCorrente, "88888");

            //assert
            var result = response.Should().BeOfType<OkObjectResult>().Subject;
            result.Value.Should().Be("88888");
        }

        [Fact(DisplayName = "MovimentaçãoContaCorrente - Returns Ok with Moviment Id - MatchIdempotency")]
        public async Task MovimentaçãoContaCorrente_ReturnsOk_withIdDoMovimento_MatchIdempotency()
        {
            //arange            
            ContaCorrente contaCorrente = new ContaCorrente("José", "555222", "12345");
            MovimentacaoConta movimentacaoContaCorrente = new MovimentacaoConta() { ContaCorrente = "12345", IdContaCorrente = "555444", TipoMovimento = EnumTipoMovimento.Crédito, Valor = 10m };
            _contaCorrenteServiceMock.Setup(s => s.BuscaContaCorrenteParaMovimentacao(movimentacaoContaCorrente)).ReturnsAsync(contaCorrente);
            _idempotencyServiceMock.Setup(s => s.ValidateIdempotency("555666")).ReturnsAsync("88888");            

            var controller = GetController();

            //act
            var response = await controller.MovimentaçãoContaCorrente(movimentacaoContaCorrente, "555666");

            //assert
            var result = response.Should().BeOfType<OkObjectResult>().Subject;
            result.Value.Should().Be("88888");
        }


        [Fact(DisplayName = "GetSaldoContaCorrente - Returns NOk - BadRequest -BuscaContaCorrente")]
        public async Task GetSaldoContaCorrente_ReturnsNOkBadRequest_inBuscaContaCorrente()
        {
            //arange
            string conta = "12345";

            var expectedException = new Exception("Erro");

            _contaCorrenteServiceMock.Setup(s => s.BuscaContaCorrente(conta)).ThrowsAsync(expectedException);
            var controller = GetController();

            //act
            var result = await controller.GetSaldoContaCorrente(conta);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var resultObject = (BadRequestObjectResult)result;
            resultObject.Value.Should().Be("Erro");
            resultObject.StatusCode.Should().Be(400);
        }

        [Fact(DisplayName = "GetSaldoContaCorrente - Returns NOk - BadRequest -AcessarSaldoContaCorrente")]
        public async Task GetSaldoContaCorrente_ReturnsNOkBadRequest_inAcessarSaldoContaCorrente()
        {

            //arange
            string conta = "12345";
            ContaCorrente contaCorrente = new ContaCorrente("José", "555222", "12345");
            Saldo saldo = new Saldo() { ContaCorrenteNumero = "12345", HoraDaConsulta = DateTime.Now, SaldoAtual = "10", TitularContaCorente = "José" };
            var expectedException = new Exception("Erro");

            _contaCorrenteServiceMock.Setup(s => s.BuscaContaCorrente(conta)).ReturnsAsync(contaCorrente);
            _contaCorrenteServiceMock.Setup(s => s.AcessarSaldoContaCorrente(contaCorrente)).ThrowsAsync(expectedException);            
            
            var controller = GetController();

            //act
            var result = await controller.GetSaldoContaCorrente(conta);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var resultObject = (BadRequestObjectResult)result;
            resultObject.Value.Should().Be("Erro");
            resultObject.StatusCode.Should().Be(400);
        }

        [Fact(DisplayName = "MovimentaçãoContaCorrente - Returns NOk - BadRequest - BuscaContaCorrenteParaMovimentacao")]
        public async Task MovimentaçãoContaCorrente_ReturnsNOkBadRequest_inBuscaContaCorrenteParaMovimentacao()
        {

            //arange            
            ContaCorrente contaCorrente = new ContaCorrente("José", "555222", "12345");
            MovimentacaoConta movimentacaoContaCorrente = new MovimentacaoConta() { ContaCorrente = "12345", IdContaCorrente = "555444", TipoMovimento = EnumTipoMovimento.Crédito, Valor = 10m };
            var expectedException = new Exception("Erro");

            _contaCorrenteServiceMock.Setup(s => s.BuscaContaCorrenteParaMovimentacao(movimentacaoContaCorrente)).ThrowsAsync(expectedException);            

            var controller = GetController();

            //act
            var result = await controller.MovimentaçãoContaCorrente(movimentacaoContaCorrente, "88888");
            
            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var resultObject = (BadRequestObjectResult)result;
            resultObject.Value.Should().Be("Erro");
            resultObject.StatusCode.Should().Be(400);
        }

        [Fact(DisplayName = "MovimentaçãoContaCorrente - Returns NOk - BadRequest - ValidateIdempotency")]
        public async Task MovimentaçãoContaCorrente_ReturnsNOkBadRequest_inValidateIdempotency()
        {

            //arange            
            ContaCorrente contaCorrente = new ContaCorrente("José", "555222", "12345");
            MovimentacaoConta movimentacaoContaCorrente = new MovimentacaoConta() { ContaCorrente = "12345", IdContaCorrente = "555444", TipoMovimento = EnumTipoMovimento.Crédito, Valor = 10m };
            var expectedException = new Exception("Erro");

            _contaCorrenteServiceMock.Setup(s => s.BuscaContaCorrenteParaMovimentacao(movimentacaoContaCorrente)).ReturnsAsync(contaCorrente);
            _idempotencyServiceMock.Setup(s => s.ValidateIdempotency("555666")).ThrowsAsync(expectedException);            

            var controller = GetController();

            //act
            var result = await controller.MovimentaçãoContaCorrente(movimentacaoContaCorrente, "555666");

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var resultObject = (BadRequestObjectResult)result;
            resultObject.Value.Should().Be("Erro");
            resultObject.StatusCode.Should().Be(400);
        }

        [Fact(DisplayName = "MovimentaçãoContaCorrente - Returns NOk - BadRequest - MovimentaContaCorrente")]
        public async Task MovimentaçãoContaCorrente_ReturnsNOkBadRequest_inMovimentaContaCorrente()
        {

            //arange            
            ContaCorrente contaCorrente = new ContaCorrente("José", "555222", "12345");
            MovimentacaoConta movimentacaoContaCorrente = new MovimentacaoConta() { ContaCorrente = "12345", IdContaCorrente = "555444", TipoMovimento = EnumTipoMovimento.Crédito, Valor = 10m };
            var expectedException = new Exception("Erro");

            _contaCorrenteServiceMock.Setup(s => s.BuscaContaCorrenteParaMovimentacao(movimentacaoContaCorrente)).ReturnsAsync(contaCorrente);
            _idempotencyServiceMock.Setup(s => s.ValidateIdempotency("555666")).ReturnsAsync("");
            _contaCorrenteServiceMock.Setup(s => s.MovimentaContaCorrente(movimentacaoContaCorrente)).ThrowsAsync(expectedException);
                        
            var controller = GetController();

            //act
            var result = await controller.MovimentaçãoContaCorrente(movimentacaoContaCorrente, "555666");

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var resultObject = (BadRequestObjectResult)result;
            resultObject.Value.Should().Be("Erro");
            resultObject.StatusCode.Should().Be(400);
        }

        [Fact(DisplayName = "MovimentaçãoContaCorrente - Returns NOk - BadRequest - AddIdempotencyControl")]
        public async Task MovimentaçãoContaCorrente_ReturnsNOkBadRequest_inAddIdempotencyControl()
        {

            //arange            
            ContaCorrente contaCorrente = new ContaCorrente("José", "555222", "12345");
            MovimentacaoConta movimentacaoContaCorrente = new MovimentacaoConta() { ContaCorrente = "12345", IdContaCorrente = "555444", TipoMovimento = EnumTipoMovimento.Crédito, Valor = 10m };
            var expectedException = new Exception("Erro");

            _contaCorrenteServiceMock.Setup(s => s.BuscaContaCorrenteParaMovimentacao(movimentacaoContaCorrente)).ReturnsAsync(contaCorrente);
            _idempotencyServiceMock.Setup(s => s.ValidateIdempotency("555666")).ReturnsAsync("");
            _contaCorrenteServiceMock.Setup(s => s.MovimentaContaCorrente(movimentacaoContaCorrente)).ReturnsAsync("88888");
            _idempotencyServiceMock.Setup(s => s.AddIdempotencyControl("555666", It.IsAny<MovimentacaoConta>(), "88888")).ThrowsAsync(expectedException);

            var controller = GetController();

            //act
            var result = await controller.MovimentaçãoContaCorrente(movimentacaoContaCorrente, "555666");

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var resultObject = (BadRequestObjectResult)result;
            resultObject.Value.Should().Be("Erro");
            resultObject.StatusCode.Should().Be(400);
        }


        private ContaCorrenteController GetController()
        {
            var controller = new ContaCorrenteController(_contaCorrenteServiceMock.Object, _idempotencyServiceMock.Object);
            return controller;
        }
    }
}
