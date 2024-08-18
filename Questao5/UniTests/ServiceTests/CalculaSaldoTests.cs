using FluentAssertions;
using Questao5.Application.Queries.Responses.GetSaldoContaCorrenteViewModel;
using Questao5.Infrastructure.Services.Calculation;
using Xunit;

namespace Questao5.UniTests.ServiceTests
{
    public class CalculaSaldoTests
    {
        [Fact(DisplayName = "CalculaSaldo - Returns correct Value")]
        public async Task CalculaSaldo()
        {
            //arrange
            List<GetSaldoContaCorrenteViewModel> list_operations = new();

            list_operations.Add(new GetSaldoContaCorrenteViewModel() { TipoMovimento = 'C', Valor = 12.50m });
            list_operations.Add(new GetSaldoContaCorrenteViewModel() { TipoMovimento = 'C', Valor = 18.50m });
            list_operations.Add(new GetSaldoContaCorrenteViewModel() { TipoMovimento = 'D', Valor = 20.30m });

            //act
            var result = CalculosSaldo.CalculaSaldoContaCorrente(list_operations);

            //assert
            result.Should().Be(10.70m);
        }
    }
}
