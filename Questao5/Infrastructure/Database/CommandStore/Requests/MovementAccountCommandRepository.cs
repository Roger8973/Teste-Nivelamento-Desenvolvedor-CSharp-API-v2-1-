using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Extensions;
using Questao5.Infrastructure.Dapper.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests.Interfaces;
using System.Data;
using System.Drawing;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class MovementAccountCommandRepository : IMovementAccountCommandRepository
    {
        private readonly IDatabase _connection;

        public MovementAccountCommandRepository(IDatabase connection)
        {
            _connection = connection;
        }
        public async Task CreateMovement(MovementAccount movementAccount)
        {
            await _connection.ExecuteAsync(@"INSERT INTO MOVIMENTO 
                                             (IDMOVIMENTO, IDCONTACORRENTE, DATAMOVIMENTO, TIPOMOVIMENTO, VALOR) 
                                             VALUES 
                                             (@IDMOVIMENTO, @IDCONTACORRENTE, @DATAMOVIMENTO, @TIPOMOVIMENTO, @VALOR)",
                                             new
                                             {
                                                 IDMOVIMENTO = movementAccount.MovementId,
                                                 IDCONTACORRENTE = movementAccount.AccountId.ToUpper(),
                                                 DATAMOVIMENTO = DateTime.Now.ToString("yyyy-MM-dd"),
                                                 TIPOMOVIMENTO = EnumExtensions.ToShortString(movementAccount.AccountMovementType),
                                                 VALOR = movementAccount.Amount
                                             });
        }
    }
}
