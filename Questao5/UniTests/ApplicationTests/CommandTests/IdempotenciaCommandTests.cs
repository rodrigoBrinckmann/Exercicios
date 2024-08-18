using MediatR;
using Moq;
using NSubstitute;
using Questao5.Application.Commands.Requests.Idempotencia;
using Questao5.Infrastructure.Database;
using Questao5.Infrastructure.Services;
using Questao5.Infrastructure.Services.Controllers;
using Xunit;

namespace Questao5.UniTests.ApplicationTests.CommandTests
{
    public class IdempotenciaCommandTests
    {
        private readonly Mock<IRepositorio> _repositorioMock = new();

        [Fact(DisplayName = "IdempotenciaCommand - CommandHandler")]
        public async Task IdempotenciaCommand()
        {
            // Arrange                        
            IdempotenciaCommand command = new IdempotenciaCommand("123456", "request", "response");
            _repositorioMock.Setup(r => r.InsertIdempotencia(command));

            var idempotenciaCommand = command;
            var idempotenciaCommandHandler = new IdempotenciaCommandHandler(_repositorioMock.Object);

            // Act
            await idempotenciaCommandHandler.Handle(command, new CancellationToken());

            // Assert
            _repositorioMock.Verify(pr => pr.InsertIdempotencia(command), Times.Once);
        }

            
    }
}
