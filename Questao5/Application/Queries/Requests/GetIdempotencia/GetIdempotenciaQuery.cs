using MediatR;

namespace Questao5.Application.Queries.Requests.GetIdempotencia
{
    public class GetIdempotenciaQuery : IRequest<string>
    {
        public string IdIdempotencia { get; set; }

        public GetIdempotenciaQuery(string idIdempotencia)
        {
            this.IdIdempotencia = idIdempotencia;
        }
    }
}
