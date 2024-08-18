namespace Questao5.Domain.Entities
{
    public class ContaCorrente
    {
        public string Nome { get; set; }
        public string IdContaCorrente { get; set; }
        public string NumeroContaCorrente { get; set; }


        public ContaCorrente(string nome, string idContaCorrente, string numeroContaCorrente)
        {
            Nome = nome;
            IdContaCorrente = idContaCorrente;
            NumeroContaCorrente = numeroContaCorrente;
        }
    }

    
}
