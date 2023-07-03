using Moq;
using Questao5.Application.Validators;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Infrastructure.Dapper.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests;

namespace ApplicationTests
{
    public class CreateMovementAccountHandlerTests
    {
        [Fact]
        public void ValidateState_ThrowsException_WhenValueIsNull()
        {
            // Arrange
            MovementAccount movementAccount = null;
            var movementValidationService = new CreateMovementAccountValidationService();

            //Act & Assert
            Assert.Throws<ArgumentNullException>(() => movementValidationService.ValidadeState(movementAccount));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ValidateState_ThrowsException_WhenAccountIdIsNullOrEmpty(string accountId)
        {
            // Arrange
            var movementAccount = new MovementAccount { AccountId = accountId };
            var movementValidationService = new CreateMovementAccountValidationService();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => movementValidationService.ValidadeState(movementAccount));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ValidateState_ThrowsException_WhenValueIsZeroOrLess(decimal ammount)
        {
            // Arrange
            var movementAccount = new MovementAccount
            {
                AccountId = "abc",
                Amount = ammount
            };

            var movementValidationService = new CreateMovementAccountValidationService();

            // Act & Assert
            Assert.Throws<NegativeOrZeroMovementException>(() => movementValidationService.ValidadeState(movementAccount));
        }

        [Fact]
        public async Task CreateMovement_WhenCalled_CheckMethodCall()
        {
            // Arrange
            var mockDb = new Mock<IDatabase>();
            var repo = new MovementAccountCommandRepository(mockDb.Object);
            var movementAccount = new MovementAccount
            {
                AccountId = "abc",
                MovementId = "123",
                AccountMovementType = AccountMovement.Debit,
                Amount = 10.0m
            };

            // Act
            await repo.CreateMovement(movementAccount);

            // Assert
            mockDb.Verify(
                db => db.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<object>()
                )
            );
        }

        [Fact]
        public async Task CreateMovement_ThrowsException_WhenToThrowAnException()
        {
            // Arrange
            var mockDb = new Mock<IDatabase>();
            var repo = new MovementAccountCommandRepository(mockDb.Object);
            var movementAccount = new MovementAccount
            {
                AccountId = "abc",
                MovementId = "123",
                AccountMovementType = AccountMovement.Debit,
                Amount = 10.0m
            };

            mockDb.Setup(
                db => db.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<object>()
                )
            ).ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => repo.CreateMovement(movementAccount));
        }
    }
}