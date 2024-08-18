using System;
using System.Globalization;

namespace Questao1 {
    class Program {
        static void Main(string[] args) {

            var conta = ContaBancaria.AbrirConta();
            ContaBancaria.Extrato(conta,true);
            ContaBancaria.Deposito(conta);
            ContaBancaria.Saque(conta);
        }
    }
}
