using Moq;
using Questao5.Application.Validators;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Infrastructure.Dapper.Interfaces;
using Questao5.Infrastructure.Database.QueryStore.Requests;

namespace ApplicationTests
{
    public class GetAccountHandlerTest
    {
        [Theory]
        [InlineData("", 0)]
        [InlineData(null, -1)]
        public void ValidateState_ThrowsException_WhenAccountIsNull(string name, int accountNumber)
        {
            // Arrange
            var accountValidationService = new GetAccountValidationService();
            var currentAccount = new CurrentAccount 
            { 
                Name = name, 
                AccountNumber = accountNumber 
            };

            // Act & Assert
            var exception = Assert.Throws<AccountIsRequiredException>(() => accountValidationService.ValidadeState(currentAccount));
            Assert.NotNull(exception);
        }

        [Fact]
        public void ValidadeAccountStatus_ThrowsException_WhenAccountTypeIsInvalid()
        {
            // Arrange
            var accountValidation = new GetAccountValidationService();

            // Act & Assert
            Assert.Throws<AccountStatusTypeNotAllowedException>(() => accountValidation.ValidadeAccountStatus((AccountStatus)999));
        }

        [Fact]
        public void ValidateState_ThrowsException_WhenAccountIsInactive()
        {
            // Arrange
            var currentAccount = new CurrentAccount
            {
                Name = "abc",
                Amount = 1,
                Status = AccountStatus.Inactive
            };

            var accountValidationService = new GetAccountValidationService();

            // Act & Assert
            Assert.Throws<AccountIsInactiveException>(() => accountValidationService.ValidadeState(currentAccount));
        }

        [Fact]
        public async Task GetStatusAccountById_WhenCalled_CheckMethodCall()
        {
            // Arrange
            var mockDb = new Mock<IDatabase>();
            var repo = new AccountQueryRepository(mockDb.Object);

            // Act
            await repo.GetStatusAccountById("id");

            // Assert
            mockDb.Verify(
                db => db.QueryFirstOrDefaultAsync<int>(
                    It.IsAny<string>(),
                    It.IsAny<object>()
                )
            );
        }

        [Fact]
        public async Task GetStatusAccountById_WhenExceptionOccurs_ThrowsException()
        {
            // Arrange
            var mockDb = new Mock<IDatabase>();
            var repo = new AccountQueryRepository(mockDb.Object);

            // Act
            await repo.GetStatusAccountById("id");

            // Assert
            mockDb.Setup(
               db => db.ExecuteAsync(
                   It.IsAny<string>(),
                   It.IsAny<object>()
               )
           ).ThrowsAsync(new Exception());
        }

        [Fact]
        public async Task GetCurrentAccountById_WhenCalled_CheckMethodCall()
        {
            // Arrange
            var mockDb = new Mock<IDatabase>();
            var repo = new AccountQueryRepository(mockDb.Object);

            // Act
            var account = await repo.GetCurrentAccountById("id");

            // Assert
            mockDb.Verify(
                db => db.QueryFirstOrDefaultAsync<CurrentAccount>(
                    It.IsAny<string>(),
                    It.IsAny<object>()
                )
            );
        }

        [Fact]
        public async Task GetCurrentAccountById_WhenExceptionOccurs_ThrowsException()
        {
            // Arrange
            var mockDb = new Mock<IDatabase>();
            var repo = new AccountQueryRepository(mockDb.Object);

            mockDb.Setup(
               db => db.QueryFirstOrDefaultAsync<CurrentAccount>(
                   It.IsAny<string>(),
                   It.IsAny<object>()
               )
           ).ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => repo.GetCurrentAccountById("id"));
        }
    }
}
