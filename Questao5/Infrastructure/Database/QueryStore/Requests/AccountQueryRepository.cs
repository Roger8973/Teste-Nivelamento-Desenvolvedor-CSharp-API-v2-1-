using Questao5.Domain.Entities;
using Questao5.Infrastructure.Dapper.Interfaces;
using Questao5.Infrastructure.Database.QueryStore.Requests.Interfaces;

namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class AccountQueryRepository : IAccountQueryRepository
    {
        private readonly IDatabase _connection;

        public AccountQueryRepository(IDatabase connection)
        {
            _connection = connection;
        }

        public async Task<bool> ValidadeExistingAccountById(string id)
        {
            return await _connection.QueryFirstOrDefaultAsync<bool>(
                   @$"SELECT COUNT (NUMERO) > 0
                      FROM CONTACORRENTE                                                         
                      WHERE IDCONTACORRENTE = @Id", new { Id = id.ToUpper() });

        }

        public async Task<int> GetStatusAccountById(string id)
        {
            return await _connection.QueryFirstOrDefaultAsync<int>(
                   @$"SELECT ATIVO AS STATUS 
                      FROM CONTACORRENTE                                                         
                      WHERE IDCONTACORRENTE = @Id", new { Id = id.ToUpper() });

        }

        public async Task<CurrentAccount> GetCurrentAccountById(string id)
        {
            return await _connection.QueryFirstOrDefaultAsync<CurrentAccount>(
                   @$"SELECT 
                        CC.NUMERO AS ACCOUNTNUMBER, CC.NOME AS NAME, CC.ATIVO AS STATUS, 
                        SUM(CASE WHEN MV.TIPOMOVIMENTO = 'C' THEN VALOR ELSE 0 END) -
                        SUM(CASE WHEN MV.TIPOMOVIMENTO = 'D' THEN VALOR ELSE 0 END) AS AMOUNT
                     FROM CONTACORRENTE CC LEFT JOIN MOVIMENTO MV ON CC.IDCONTACORRENTE = MV.IDCONTACORRENTE                                                  
                     WHERE CC.IDCONTACORRENTE = @Id", new { Id = id.ToUpper() });

        }
    }
}
