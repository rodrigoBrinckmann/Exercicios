using Questao5.Application.Queries.Responses.GetSaldoContaCorrenteViewModel;

namespace Questao5.Infrastructure.Services.Calculation
{
    public static class CalculosSaldo

    {        
        public static decimal CalculaSaldoContaCorrente(List<GetSaldoContaCorrenteViewModel> ocorrencias)
        {
            //SALDO = SOMA_DOS_CREDITOS – SOMA_DOS_DEBITOS
            decimal saldo = 0m;
            foreach (var data in ocorrencias)
            {
                if (data.TipoMovimento == 'C')
                    saldo += data.Valor;
                else if (data.TipoMovimento == 'D')
                    saldo -= data.Valor;            
            }
            return saldo;
        }
    }
}
