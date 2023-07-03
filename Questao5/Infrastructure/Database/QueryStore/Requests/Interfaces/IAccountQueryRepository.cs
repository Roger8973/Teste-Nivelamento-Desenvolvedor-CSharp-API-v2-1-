using Questao5.Domain.Entities;


namespace Questao5.Infrastructure.Database.QueryStore.Requests.Interfaces
{
    public interface IAccountQueryRepository
    {
        Task<bool> ValidadeExistingAccountById(string id);
        Task<int> GetStatusAccountById(string id);
        Task<CurrentAccount> GetCurrentAccountById(string id);
    }
}
