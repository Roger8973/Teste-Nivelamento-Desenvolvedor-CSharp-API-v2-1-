using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace Questao5.Application.Commands.Requests
{
    public class CreateMovementAccountRequest : IRequest<CreateMovementAccountResponse>
    {
        [Required]
        public string IdempotencyKey { get; set; }
        [Required]
        public string AccountId { get; set; }
        public AccountMovement AccountMovementType { get; set; }
        public decimal Amount { get; set; }
    }
}
