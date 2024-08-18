using FluentAssertions;
using Moq;
using Questao5.Application.Queries.Requests.GetContaCorrente;
using Questao5.Application.Queries.Requests.GetSaldoContaCorrente;
using Questao5.Application.Queries.Responses.GetContaCorrenteViewModel;
using Questao5.Application.Queries.Responses.GetSaldoContaCorrenteViewModel;
using Questao5.Infrastructure.Database;
using Xunit;

namespace Questao5.UniTests.ApplicationTests.QueryTests
{
    public class GetSaldoContaCorrenteQueryTests
    {
        private readonly Mock<IRepositorio> _repositorioMock = new();

        [Fact(DisplayName = "GetSaldoContaCorrenteQuery - QueryHandler")]
        public async Task GetSaldoContaCorrenteQuery()
        {
            // Arrange                        
            GetSaldoContaCorrenteQuery query = new GetSaldoContaCorrenteQuery("12345");
            List<GetSaldoContaCorrenteViewModel> expectedResult = new List<GetSaldoContaCorrenteViewModel>()
            {
                new GetSaldoContaCorrenteViewModel () {Valor = 10m, TipoMovimento = 'C'},
                new GetSaldoContaCorrenteViewModel () {Valor = 9m, TipoMovimento = 'D'},
            };            
            _repositorioMock.Setup(r => r.GetSaldoContaCorrente(query)).ReturnsAsync(expectedResult);

            var getSaldoContaCorrenteQuery = query;
            var getSaldoContaCorrenteQueryHandler = new GetSaldoContaCorrenteQueryHandler(_repositorioMock.Object);

            // Act
            var result = await getSaldoContaCorrenteQueryHandler.Handle(query, new CancellationToken());

            // Assert            
            _repositorioMock.Verify(pr => pr.GetSaldoContaCorrente(query), Times.Once);
            result.Should().NotBeEmpty();            
        }
    }
}
