using MediatR;
using Questao5.Application.Queries.Responses;
using System.ComponentModel.DataAnnotations;

namespace Questao5.Application.Queries.Requests
{
    public class GetCurrentAccountRequest : IRequest<GetCurrentAccountResponse>
    {
        [Required]
        public string AccountId { get; set; }
    }
}
