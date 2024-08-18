using Questao5.Application.Queries.Responses.GetContaCorrenteViewModel;
using Questao5.Domain.Constants;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Services.Exceptions;

namespace Questao5.Infrastructure.Services.Validators
{
    public static class ValidaDados
    {
        public static void ValidaContaCorrente(GetContaCorrenteViewModel request, string tipoConsulta)
        {
            if (request == null)
            {                
                var message = tipoConsulta == "saldo" ? AppConstants.invalid_account_saldo : AppConstants.invalid_account_movimentacao;
                throw new ContaCorrenteExceptions(message);
            }
            else if (!request.Ativo)
            {
                var message = tipoConsulta == "saldo" ? AppConstants.inactive_account_saldo : AppConstants.inactive_account_movimentacao;
                throw new ContaCorrenteExceptions(message);
            }
        }

        public static void ValidaMovimentacao(MovimentacaoConta request)
        {
            if (request.Valor <= 0)
            {
                var message = AppConstants.invalid_value;
                throw new MovimentacaoException(message);
            }
            else if (request.TipoMovimento.ToString() != "Débito" && request.TipoMovimento.ToString() != "Crédito")
            {
                var message = AppConstants.invalid_type;
                throw new MovimentacaoException(message);
            }            
        }
    }
}
