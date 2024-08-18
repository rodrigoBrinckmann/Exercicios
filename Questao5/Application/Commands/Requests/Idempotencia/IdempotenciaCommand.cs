using MediatR;

namespace Questao5.Application.Commands.Requests.Idempotencia
{
    public class IdempotenciaCommand: IRequest<Unit>
    {
        public string IdIdempotencia { get; }
        public object Request {  get; }
        public object Response { get; }

        public IdempotenciaCommand(string idIdempotencia, object request, object response)
        {
            IdIdempotencia = idIdempotencia;
            Request = request;
            Response = response;
        }
    }
}
