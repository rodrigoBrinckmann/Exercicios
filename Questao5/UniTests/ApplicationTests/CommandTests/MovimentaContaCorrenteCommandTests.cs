using Moq;
using Questao5.Application.Commands.Requests.Idempotencia;
using Questao5.Application.Commands.Requests.MovimentaContaCorrente;
using Questao5.Infrastructure.Database;
using Xunit;

namespace Questao5.UniTests.ApplicationTests.CommandTests
{
    public class MovimentaContaCorrenteCommandTests
    {
        private readonly Mock<IRepositorio> _repositorioMock = new();

        [Fact(DisplayName = "MovimentaContaCorrenteCommand - CommandHandler")]
        public async Task MovimentaContaCorrenteCommand()
        {
            // Arrange                        
            MovimentaContaCorrenteCommand command = new MovimentaContaCorrenteCommand() { ContaId = "123-456", HoraMovimentação = DateTime.Now.ToString("dd/MM/yyyy"), IdMovimento =new Guid(), TipoMovimento = 'C', Valor = "80"};
            _repositorioMock.Setup(r => r.InsertMovimentoContaCorrente(command));

            var movimentaContaCorrenteCommand = command;
            var movimentaContaCorrenteCommandHandler = new MovimentaContaCorrenteCommandHandler(_repositorioMock.Object);

            // Act
            await movimentaContaCorrenteCommandHandler.Handle(command, new CancellationToken());

            // Assert            
            _repositorioMock.Verify(pr => pr.InsertMovimentoContaCorrente(command), Times.Once);            
        }
    }
}
