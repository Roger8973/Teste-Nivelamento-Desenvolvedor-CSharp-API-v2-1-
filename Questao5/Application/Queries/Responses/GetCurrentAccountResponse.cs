using Questao5.Application.Dtos;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Queries.Responses
{
    public class GetCurrentAccountResponse : Response
    {
       public CurrentAccountDto Data { get; set; }
    }
}
