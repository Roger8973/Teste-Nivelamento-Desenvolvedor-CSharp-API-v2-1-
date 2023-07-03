namespace Questao5.Infrastructure.Dapper.Interfaces
{
    public interface IDatabase
    {
        Task ExecuteAsync(string sql, object parameters = null);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters= null);
    }
}
