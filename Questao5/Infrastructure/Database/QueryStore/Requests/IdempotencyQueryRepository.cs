using Questao5.Infrastructure.Dapper.Interfaces;
using Questao5.Infrastructure.Database.QueryStore.Requests.Interfaces;

namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class IdempotencyQueryRepository : IIdempotencyQueryRepository
    {
        private readonly IDatabase _connection;

        public IdempotencyQueryRepository(IDatabase connection)
        {
            _connection = connection;
        }
        public async Task<string> GetIdempotency(string indepotencyKey)
        {
            return await _connection.QueryFirstOrDefaultAsync<string>(@"SELECT RESULTADO FROM IDEMPOTENCIA 
                                                                       WHERE CHAVE_IDEMPOTENCIA = @CHAVE_IDEMPOTENCIA",
                                                                        new { CHAVE_IDEMPOTENCIA = indepotencyKey });
        }
    }
}
