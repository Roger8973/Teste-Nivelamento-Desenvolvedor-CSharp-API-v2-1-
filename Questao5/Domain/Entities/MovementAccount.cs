using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities
{
    public class MovementAccount
    {
        public string MovementId { get; set; }
        public string AccountId { get; set; }
        public AccountMovement AccountMovementType { get; set; }
        public decimal Amount { get; set; }
    }
}
