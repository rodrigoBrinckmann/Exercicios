using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests.MovimentaContaCorrente;
using Questao5.Application.Queries.Requests.GetContaCorrente;
using Questao5.Application.Queries.Requests.GetSaldoContaCorrente;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Services.Validators;
using Questao5.Infrastructure.Services.Calculation;
using FluentAssertions.Equivalency;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
using System.Globalization;
using Newtonsoft.Json;
using System.Text;
using Questao5.Application.Queries.Requests.GetIdempotencia;
using Questao5.Application.Commands.Requests.Idempotencia;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Questao5.Infrastructure.Services.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ContaCorrenteController : ControllerBase
    {        
        private readonly IContaCorrenteService _movimentaçãoContaCorrenteService;
        private readonly IIdempotencyService _idempotencyService;

        public ContaCorrenteController(IContaCorrenteService movimentaçãoContaCorrenteService, IIdempotencyService idempotencyService)
        {     
            _movimentaçãoContaCorrenteService = movimentaçãoContaCorrenteService;
            _idempotencyService = idempotencyService;
        }

        /// <summary>
        /// Consultar saldo da conta corrente
        /// </summary>
        /// <param name="contaNumero"></param>
        /// <remarks>
        /// https://localhost:7140/api/getsaldo/'contaNumero'
        /// </remarks>
        /// <returns>
        /// Valida se conta está cadastrada
        /// Valida se conta está ativa
        /// Caso dados estejam válidos, retorna
        /// •	Número da conta corrente
        /// •	Nome do titular da conta corrente
        /// •	Data e hora da resposta da consulta
        /// •	Valor do Saldo atual
        /// Caso dados inválidos, retorna Status Code 400
        /// </returns>
        [HttpGet("getsaldo/{contaNumero}")]
        public async Task<IActionResult> GetSaldoContaCorrente(string contaNumero)
        {
            try
            {
                var contaCorrente = await _movimentaçãoContaCorrenteService.BuscaContaCorrente(contaNumero);
                var saldo = await _movimentaçãoContaCorrenteService.AcessarSaldoContaCorrente(contaCorrente);
                return Ok(saldo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Fazer movimentações na conta corrente
        /// </summary>
        /// <param name="dadosMovimentacao"></param>
        /// <remarks>
        /// {"contaCorrente": "string", "tipoMovimento": "Crédito"/0 ou "Débito"/1,"valor": 0}        
        /// </remarks>
        /// <returns>
        /// Valida se conta está cadastrada
        /// Valida se conta está ativa
        /// Valida valores passados,se são positivo
        /// Valida se tipo de Movimento é 'Débito' ou 'Crédito'
        /// Caso dados estejam válidos, retorna
        /// •	Número do Id do Movimento gerado
        /// Caso dados inválidos, retorna Status Code 400
        /// Valida idempotência caso idempotencyKey enviada no Header já tenha sido utilizada - neste caso, retorna o número da transação
        /// </returns>

        [HttpPost("MovimentaConta")]
        public async Task<IActionResult> MovimentaçãoContaCorrente([FromBody] MovimentacaoConta dadosMovimentacao, [FromHeader] string idempotencyKey)
        {
            try
            {
                var contaCorrente = await _movimentaçãoContaCorrenteService.BuscaContaCorrenteParaMovimentacao(dadosMovimentacao);
                                                
                var idempotenciaValidation = await _idempotencyService.ValidateIdempotency(idempotencyKey);

                if (string.IsNullOrEmpty(idempotenciaValidation))
                {
                    var idMovimento = await _movimentaçãoContaCorrenteService.MovimentaContaCorrente(dadosMovimentacao);
                    await _idempotencyService.AddIdempotencyControl(idempotencyKey, dadosMovimentacao, idMovimento);

                    return Ok(idMovimento);
                }
                return Ok(idempotenciaValidation);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }  
        }
        
    }
}
