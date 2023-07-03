using Questao2.Models;

namespace Questao2.Rest.Interfaces
{
    public interface IRestService
    {
        Task<Response> GetAsync(string team, int year, int page);
    }
}
