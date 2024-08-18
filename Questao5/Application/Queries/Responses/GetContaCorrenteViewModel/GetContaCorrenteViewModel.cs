namespace Questao5.Application.Queries.Responses.GetContaCorrenteViewModel
{
    public class GetContaCorrenteViewModel
    {
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public string IdContaCorrente { get; }

        public GetContaCorrenteViewModel()
        {
            
        }
        public GetContaCorrenteViewModel(string nome, bool ativo, string idContaCorrente)
        {
            Nome = nome;
            Ativo = ativo;
            IdContaCorrente = idContaCorrente;
        }
    }
}
