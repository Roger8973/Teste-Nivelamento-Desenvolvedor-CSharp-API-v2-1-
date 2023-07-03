using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Validators.Interfaces
{
    public interface ICreateMovementAccountValidationService
    {
        void ValidadeState(MovementAccount movementAccount);
    }
}
