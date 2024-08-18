using FluentAssertions;
using Moq;
using Questao5.Application.Commands.Requests.MovimentaContaCorrente;
using Questao5.Application.Queries.Requests.GetContaCorrente;
using Questao5.Application.Queries.Responses.GetContaCorrenteViewModel;
using Questao5.Infrastructure.Database;
using Xunit;

namespace Questao5.UniTests.ApplicationTests.QueryTests
{
    public class GetContaCorrenteQueryTests
    {
        private readonly Mock<IRepositorio> _repositorioMock = new();

        [Fact(DisplayName = "GetContaCorrenteQuery - QueryHandler")]
        public async Task GetContaCorrenteQuery()
        {
            // Arrange                        
            GetContaCorrenteQuery query = new GetContaCorrenteQuery("555");
            GetContaCorrenteViewModel expectedResult = new GetContaCorrenteViewModel("Pedro", true, "1234554321");
            _repositorioMock.Setup(r => r.GetContaCorrente(query)).ReturnsAsync(expectedResult);
            
            var getContaCorrenteQuery = query;
            var getContaCorrenteQueryHandler = new GetContaCorrenteQueryHandler(_repositorioMock.Object);

            // Act
            var result = await getContaCorrenteQueryHandler.Handle(query, new CancellationToken());

            // Assert            
            _repositorioMock.Verify(pr => pr.GetContaCorrente(query), Times.Once);
            result.Ativo.Should().Be(expectedResult.Ativo);
            result.Nome.Should().Be(expectedResult.Nome);
            result.IdContaCorrente.Should().Be(expectedResult.IdContaCorrente);
        }
    }
}
