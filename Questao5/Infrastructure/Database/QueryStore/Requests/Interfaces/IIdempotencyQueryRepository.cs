namespace Questao5.Infrastructure.Database.QueryStore.Requests.Interfaces
{
    public interface IIdempotencyQueryRepository
    {
        Task<string> GetIdempotency(string indepotencyKey);
    }
}
