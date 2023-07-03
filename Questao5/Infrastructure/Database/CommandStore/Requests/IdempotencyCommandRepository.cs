using Questao5.Domain.Entities;
using Questao5.Infrastructure.Dapper.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests.Interfaces;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class IdempotencyCommandRepository : IIdempotencyCommandRepository
    {
        private readonly IDatabase _connection;

        public IdempotencyCommandRepository(IDatabase connection)
        {
            _connection = connection;
        }

        public async Task CreateIdempotency(Idempotency idempotency)
        {
            await _connection.ExecuteAsync(@"INSERT INTO IDEMPOTENCIA
                                            (CHAVE_IDEMPOTENCIA, REQUISICAO, RESULTADO)
                                            VALUES(@CHAVE_IDEMPOTENCIA, @REQUISICAO, @RESULTADO)",
                                            new
                                            {
                                                CHAVE_IDEMPOTENCIA = idempotency.IdempotencyId,
                                                REQUISICAO = idempotency.Request,
                                                RESULTADO = idempotency.Result
                                            });
        }    
    }
}
