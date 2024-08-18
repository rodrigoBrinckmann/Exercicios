using FluentAssertions;
using Moq;
using Questao5.Application.Queries.Requests.GetContaCorrente;
using Questao5.Application.Queries.Requests.GetIdempotencia;
using Questao5.Application.Queries.Responses.GetContaCorrenteViewModel;
using Questao5.Infrastructure.Database;
using Xunit;

namespace Questao5.UniTests.ApplicationTests.QueryTests
{
    public class GetIdempotenciaQueryTests
    {
        private readonly Mock<IRepositorio> _repositorioMock = new();

        [Fact(DisplayName = "GetIdempotenciaQuery - QueryHandler")]
        public async Task GetIdempotenciaQuery()
        {
            // Arrange                        
            GetIdempotenciaQuery query = new GetIdempotenciaQuery("55555");
            string expectedResult = "88888";            
            _repositorioMock.Setup(r => r.GetIdempotencia(query)).ReturnsAsync(expectedResult);

            var getIdempotenciaQuery = query;
            var getIdempotenciaQueryHandler = new GetIdempotenciaQueryHandler(_repositorioMock.Object);

            // Act
            var result = await getIdempotenciaQueryHandler.Handle(query, new CancellationToken());

            // Assert            
            _repositorioMock.Verify(pr => pr.GetIdempotencia(query), Times.Once);
            result.Should().Be(expectedResult);                        
        }
    }
}
