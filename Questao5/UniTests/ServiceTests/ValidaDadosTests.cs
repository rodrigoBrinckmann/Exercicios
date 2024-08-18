using FluentAssertions;
using Questao5.Application.Queries.Responses.GetContaCorrenteViewModel;
using Questao5.Application.Queries.Responses.GetSaldoContaCorrenteViewModel;
using Questao5.Domain.Constants;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Services.Calculation;
using Questao5.Infrastructure.Services.Exceptions;
using Questao5.Infrastructure.Services.Validators;
using System;
using Xunit;

namespace Questao5.UniTests.ServiceTests
{
    public class ValidaDadosTests
    {
        [Fact(DisplayName = "ValidaContaCorrente - conta não encontrada")]
        public async Task ValidaContaCorrente_contaNaoEncontrada()
        {            
            //arrange
            GetContaCorrenteViewModel getContaCorrenteViewModel = new();
            string tipoConsulta = "saldo";
            //assert
            Exception exception = Assert.Throws<ContaCorrenteExceptions>(() => ValidaDados.ValidaContaCorrente(null, tipoConsulta));
            Assert.Equal(AppConstants.invalid_account_saldo, exception.Message);
        }

        [Fact(DisplayName = "ValidaContaCorrente - conta inativa")]
        public async Task ValidaContaCorrente_contaInativa()
        {            
            //arrange
            GetContaCorrenteViewModel getContaCorrenteViewModel = new GetContaCorrenteViewModel() { Ativo = false, Nome = "José" };
            string tipoConsulta = "saldo";
            //assert
            Exception exception = Assert.Throws<ContaCorrenteExceptions>(() => ValidaDados.ValidaContaCorrente(getContaCorrenteViewModel, tipoConsulta));
            Assert.Equal(AppConstants.inactive_account_saldo, exception.Message);
        }

        [Fact(DisplayName = "ValidaMovimentacao - valor menor que 0")]
        public async Task ValidaMovimentacao_valorInválido()
        {            
            //arrange
            MovimentacaoConta movimentacaoConta = new MovimentacaoConta() { ContaCorrente = "456", Valor = 0};            
            //assert
            Exception exception = Assert.Throws<MovimentacaoException>(() => ValidaDados.ValidaMovimentacao(movimentacaoConta));
            Assert.Equal(AppConstants.invalid_value, exception.Message);
        }
    }
}
