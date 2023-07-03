using Newtonsoft.Json.Linq;
using Questao5.Application.Validators.Interfaces;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;

namespace Questao5.Application.Validators
{
    public class CreateMovementAccountValidationService : ICreateMovementAccountValidationService
    {
        public void ValidadeState(MovementAccount movementAccount)
        {
            if (movementAccount == null)
                throw new ArgumentNullException(nameof(movementAccount));

            if (string.IsNullOrEmpty(movementAccount.AccountId))
                throw new ArgumentNullException(nameof(movementAccount));

            if (movementAccount.AccountMovementType != AccountMovement.Debit
                && movementAccount.AccountMovementType != AccountMovement.Credit)
                throw new InvalidMovementTypeException();

            if (movementAccount.Amount <= 0)
                throw new NegativeOrZeroMovementException();

            if (movementAccount.Amount > 1000000)
                throw new MotionExceptionAboveAllowedValue();
        }
    }
}
