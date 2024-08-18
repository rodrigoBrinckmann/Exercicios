namespace Questao5.Domain.Constants
{
    public static class AppConstants
    {
        #region saldo
        public const string invalid_account_saldo = "Apenas contas correntes cadastradas podem consultar o saldo";
        public const string inactive_account_saldo = "Apenas contas correntes ativas podem consultar o saldo";
        #endregion

        #region movimentacao
        public const string invalid_account_movimentacao = "Apenas contas correntes cadastradas podem receber movimentação";
        public const string inactive_account_movimentacao = "Apenas contas correntes ativas podem receber movimentação";

        public const string invalid_value = "Apenas valores positivos podem ser recebidos";
        public const string invalid_type = "Apenas os tipos “Débito” ou “Crédito” podem ser aceitos";
        #endregion

    }
}
