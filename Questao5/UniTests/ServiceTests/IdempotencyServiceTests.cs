using FluentAssertions;
using MediatR;
using Moq;
using Questao5.Application.Commands.Requests.Idempotencia;
using Questao5.Application.Queries.Requests.GetContaCorrente;
using Questao5.Application.Queries.Requests.GetIdempotencia;
using Questao5.Application.Queries.Responses.GetContaCorrenteViewModel;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Services;
using System;
using Xunit;

namespace Questao5.UniTests.ServiceTests
{
    public class IdempotencyServiceTests
    {
        private readonly Mock<IMediator> _mediatorMock = new();

        public IdempotencyServiceTests()
        {           
        }

        [Fact(DisplayName = "IdempotencyService - AddIdempotencyControl")]
        public async Task IdempotencyService_AddIdempotencyControl()
        {
            //arrange
            string idempotencyKey = "idemKey123";
            MovimentacaoConta movimentacaoConta = new MovimentacaoConta() { ContaCorrente = "555", TipoMovimento = 0, Valor = 10m };
            string response = "Example of response";
            
            _mediatorMock.Setup(s => s.Send<Unit>(It.IsAny<IdempotenciaCommand>(), new CancellationToken())).ReturnsAsync(new Unit());

            var service = new IdempotencyService(_mediatorMock.Object);

            //act
            await service.AddIdempotencyControl(idempotencyKey, movimentacaoConta, response);

            //assert - no return - no errors
        }

        [Fact(DisplayName = "IdempotencyService - ValidateIdempotency")]
        public async Task IdempotencyService_ValidateIdempotency()
        {
            //arrange
            string idempotencyKey = "idemKey123";

            _mediatorMock.Setup(s => s.Send<string>(It.IsAny<GetIdempotenciaQuery>(), new CancellationToken())).ReturnsAsync(idempotencyKey);

            var service = new IdempotencyService(_mediatorMock.Object);

            //act
            var result = await service.ValidateIdempotency(idempotencyKey);

            //assert
            result.Should().Be(idempotencyKey);            
        }
    }
}
