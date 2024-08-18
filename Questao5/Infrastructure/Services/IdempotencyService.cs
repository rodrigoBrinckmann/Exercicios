using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests.Idempotencia;
using Questao5.Application.Queries.Requests.GetIdempotencia;

namespace Questao5.Infrastructure.Services
{

    public interface IIdempotencyService
    {
        Task<string> ValidateIdempotency(string idempotencyKey);
        Task AddIdempotencyControl(string idempotencyKey, object request, object response);    
    }

    public class IdempotencyService : IIdempotencyService
    {
        private readonly IMediator _mediator;
        public IdempotencyService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task AddIdempotencyControl(string idempotencyKey, object request, object response)
        {
            var queryIdempotentCommand = new IdempotenciaCommand(idempotencyKey, JsonConvert.SerializeObject(request), response);
            await _mediator.Send(queryIdempotentCommand);
        }

        public async Task<string> ValidateIdempotency(string idempotencyKey)
        {
            var queryIdempotent = new GetIdempotenciaQuery(idempotencyKey);
            var idempotenciaValidation = await _mediator.Send(queryIdempotent);
            return idempotenciaValidation;
            
        }
    }
}
