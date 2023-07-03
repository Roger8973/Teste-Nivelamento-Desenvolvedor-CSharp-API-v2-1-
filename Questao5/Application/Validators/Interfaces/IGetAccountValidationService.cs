using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Validators.Interfaces
{
    public interface IGetAccountValidationService
    {
        void ValidadeExistingAccount(bool isAccount);
        void ValidadeState(CurrentAccount currentAccount);
        void ValidadeAccountStatus(AccountStatus accountStatusType);
    }
}
