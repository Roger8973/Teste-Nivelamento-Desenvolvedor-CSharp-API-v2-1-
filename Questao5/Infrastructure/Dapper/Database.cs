using Dapper;
using Questao5.Infrastructure.Dapper.Interfaces;
using System.Data;

namespace Questao5.Infrastructure.Dapper
{
    public class Database : IDatabase
    {
        private readonly IDbConnection _connection;

        public Database(IDbConnection connection)
        {
            _connection = connection;
        }

        public Task ExecuteAsync(string sql, object parameters = null)
            => _connection.ExecuteAsync(sql, parameters);

        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
            => _connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
    }
}
