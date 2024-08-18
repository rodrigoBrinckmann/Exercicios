using Dapper;
using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Application.Commands.Requests.Idempotencia;
using Questao5.Application.Commands.Requests.MovimentaContaCorrente;
using Questao5.Application.Queries.Requests.GetContaCorrente;
using Questao5.Application.Queries.Requests.GetIdempotencia;
using Questao5.Application.Queries.Requests.GetSaldoContaCorrente;
using Questao5.Application.Queries.Responses.GetContaCorrenteViewModel;
using Questao5.Application.Queries.Responses.GetSaldoContaCorrenteViewModel;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database
{

    public interface IRepositorio
    {
        Task<GetContaCorrenteViewModel> GetContaCorrente(GetContaCorrenteQuery request);
        Task<string> GetIdempotencia(GetIdempotenciaQuery request);
        Task<List<GetSaldoContaCorrenteViewModel>> GetSaldoContaCorrente(GetSaldoContaCorrenteQuery request);
        Task InsertIdempotencia(IdempotenciaCommand request);
        Task InsertMovimentoContaCorrente(MovimentaContaCorrenteCommand request);

    }

    public class Repositorio: IRepositorio
    {
        private readonly DatabaseConfig _databaseConfig;

        public Repositorio(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<GetContaCorrenteViewModel> GetContaCorrente(GetContaCorrenteQuery request)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            {
                var script = "select * from contacorrente where numero=@NumeroConta";
                connection.Open();
                var conta = await connection.QueryFirstOrDefaultAsync<GetContaCorrenteViewModel>(script, request);
                return conta;
            }
        }

        public async Task<string> GetIdempotencia(GetIdempotenciaQuery request)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            {
                var script = "select resultado from idempotencia where chave_idempotencia=@IdIdempotencia";
                connection.Open();
                var conta = await connection.QueryFirstOrDefaultAsync<string>(script, request);
                return conta;
            }
        }

        public async Task<List<GetSaldoContaCorrenteViewModel>> GetSaldoContaCorrente(GetSaldoContaCorrenteQuery request)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            {
                var script = "select * from movimento where idcontacorrente=@IdConta";
                connection.Open();
                var conta = await connection.QueryAsync<GetSaldoContaCorrenteViewModel>(script, request);
                return conta.ToList();
            }
        }

        public async Task InsertIdempotencia(IdempotenciaCommand request)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            {
                var script = "insert into idempotencia(chave_idempotencia, requisicao, resultado) values (@IdIdempotencia,@Request,@Response)";
                connection.Open();
                var idempotencia = await connection.ExecuteAsync(script, request);
            }
        }

        public async Task InsertMovimentoContaCorrente(MovimentaContaCorrenteCommand request)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            {
                var script = "insert into movimento(idmovimento, idcontacorrente, datamovimento,tipomovimento,valor) values (@IdMovimento,@ContaId,@HoraMovimentação,@TipoMovimento,@Valor)";
                connection.Open();
                var movimentacao = await connection.ExecuteAsync(script, request);                
            }
        }
    }
}
