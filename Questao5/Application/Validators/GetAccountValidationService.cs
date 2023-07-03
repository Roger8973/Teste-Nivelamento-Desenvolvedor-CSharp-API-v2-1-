using Questao5.Application.Validators.Interfaces;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using System.Security.Principal;

namespace Questao5.Application.Validators
{
    public class GetAccountValidationService : IGetAccountValidationService
    {
        public void ValidadeState(CurrentAccount currentAccount)
        {
            if (string.IsNullOrEmpty(currentAccount.Name) 
                && currentAccount.AccountNumber <= 0)
                throw new AccountIsRequiredException();

            if (currentAccount.Status == AccountStatus.Inactive)
                throw new AccountIsInactiveException();
        }
        public void ValidadeAccountStatus(AccountStatus accountStatusType)
        {
            if (accountStatusType != AccountStatus.Inactive
                && accountStatusType != AccountStatus.Active)
                throw new AccountStatusTypeNotAllowedException();

            if (accountStatusType == AccountStatus.Inactive)
                throw new AccountIsInactiveException();
        }

        public void ValidadeExistingAccount(bool isAccount)
        {
            if (!isAccount)
                throw new AccountIsRequiredException();
        }
    }
}
