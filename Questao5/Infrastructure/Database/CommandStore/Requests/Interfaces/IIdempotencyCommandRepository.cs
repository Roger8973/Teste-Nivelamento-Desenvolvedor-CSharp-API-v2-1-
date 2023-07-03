using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.CommandStore.Requests.Interfaces
{
    public interface IIdempotencyCommandRepository
    {
        Task CreateIdempotency(Idempotency indepotency);
    }
}
