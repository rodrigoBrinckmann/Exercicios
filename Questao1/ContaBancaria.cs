using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria {

        public int NumeroConta { get;}
        public string Titular { get; set; }
        public double DepositoInicial { get; set; }
        private double _Saldo { get; set; }

        public ContaBancaria(int conta, string titular, double deposito)
        {
            this.NumeroConta = conta;
            this.Titular = titular;
            this.DepositoInicial = DepositoInicial;
            this._Saldo = deposito;
        }

        public ContaBancaria(int conta, string titular)
        {
            this.NumeroConta = conta;
            this.Titular = titular;
            this._Saldo = 0;
        }        

        public static ContaBancaria AbrirConta()
        {
            ContaBancaria conta;

            Console.Write("Entre o número da conta: ");
            int numero = int.Parse(Console.ReadLine());
            Console.Write("Entre o titular da conta: ");
            string titular = Console.ReadLine();
            Console.Write("Haverá depósito inicial (s/n)? ");
            char resp = char.Parse(Console.ReadLine());
            if (resp == 's' || resp == 'S')
            {
                Console.Write("Entre o valor de depósito inicial: ");
                double depositoInicial = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                return conta = new ContaBancaria(numero, titular, depositoInicial);
            }
            else
            {
                return conta = new ContaBancaria(numero, titular);
            }
        }

        public static void Extrato(ContaBancaria conta, bool novaConta = false)
        {
            if(novaConta) 
                Console.WriteLine();            
            string cabeçalho = novaConta ? Constants.MensagemExtratoNovo : Constants.MensagemExtratoAtualização;
            Console.WriteLine(cabeçalho);
            Console.Write($"Conta: {conta.NumeroConta}, ");
            Console.Write($"Titular: {conta.Titular}, ");            
            Console.WriteLine($"Saldo: $ {conta._Saldo.ToString("0.00", CultureInfo.InvariantCulture)}");
        }

        public static void Deposito(ContaBancaria conta)
        {
            Console.WriteLine();
            Console.Write("Entre um valor para depósito: ");
            double quantia = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            conta._Saldo += quantia;
            //Console.WriteLine("Dados da conta atualizados:");
            //Console.WriteLine(conta);
            Extrato(conta);
        }

        public static void Saque(ContaBancaria conta)
        {
            Console.WriteLine();
            Console.Write("Entre um valor para saque: ");
            double quantia = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            if (conta._Saldo > quantia)
            {
                conta._Saldo -= quantia + Constants.TaxaSaque;
            }
            else
            {
                Console.WriteLine("Saldo insuficiente!");
            }
            //Console.WriteLine("Dados da conta atualizados:");
            //Console.WriteLine(conta);
            Extrato(conta);            
        }

    }
}
