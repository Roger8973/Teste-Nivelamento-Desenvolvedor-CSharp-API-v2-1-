using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Dtos;
using Questao5.Application.Validators.Interfaces;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Domain.Extensions;
using Questao5.Infrastructure.Database.CommandStore.Requests.Interfaces;
using Questao5.Infrastructure.Database.QueryStore.Requests.Interfaces;

namespace Questao5.Application.Handlers
{
    public class CreateMovementAccountHandler : IRequestHandler<CreateMovementAccountRequest, CreateMovementAccountResponse>
    {
        private readonly IMovementAccountCommandRepository _movementAccountCommandRepository;
        private readonly IIdempotencyCommandRepository _indepotencyCommandRepository;
        private readonly IAccountQueryRepository _accountQueryRepository;
        private readonly IIdempotencyQueryRepository _idempotencyQueryRepository;
        private readonly ICreateMovementAccountValidationService _movementValidationService;
        private readonly IGetAccountValidationService _accountValidationService;
        private readonly IMapper _mapper;

        public CreateMovementAccountHandler(IMovementAccountCommandRepository movementAccountQueryRepository,
                                            IAccountQueryRepository accountQueryRepository,
                                            ICreateMovementAccountValidationService movementValidationService,
                                            IGetAccountValidationService accountValidationService,
                                            IIdempotencyCommandRepository indepotencyCommandRepository,
                                            IIdempotencyQueryRepository idempotencyQueryRepository,
                                            IMapper mapper
                                           )
        {
            _movementAccountCommandRepository = movementAccountQueryRepository;
            _accountQueryRepository = accountQueryRepository;
            _movementValidationService = movementValidationService;
            _accountValidationService = accountValidationService;
            _indepotencyCommandRepository = indepotencyCommandRepository;
            _idempotencyQueryRepository = idempotencyQueryRepository;
            _mapper = mapper;
        }

        public async Task<CreateMovementAccountResponse> Handle(CreateMovementAccountRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var idempotencyResult = await TryGetIdempotentResult(request);

                if (idempotencyResult != null)
                    return idempotencyResult;

                var movement = _mapper.Map<MovementAccount>(request);
                ValidateMovement(movement);

                var isAccount = await _accountQueryRepository.ValidadeExistingAccountById(movement.AccountId);
                ValidadeExistingAccount(isAccount);

                var statusAccount = await _accountQueryRepository.GetStatusAccountById(movement.AccountId);
                ValidateAccountStatus(EnumExtensions.ForEnum(statusAccount));

                movement.MovementId = Guid.NewGuid().ToString().ToUpper();

                await _movementAccountCommandRepository.CreateMovement(movement);

                await CreateIdempotentEntry(request, movement.MovementId);

                return new CreateMovementAccountResponse
                {
                    Success = true,
                    Data = new CreateMovementAccountDto 
                    { 
                        MovementId = movement.MovementId 
                    }
                };
            }
            catch (AccountIsRequiredException)
            {
                return new CreateMovementAccountResponse
                {
                    Success = false,
                    Error = new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.INVALID_ACCOUNT,
                        Message = "The current account not found."
                    }
                };
            }
            catch (AccountIsInactiveException)
            {
                return new CreateMovementAccountResponse
                {
                    Success = false,
                    Error = new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.INACTIVE_ACCOUNT,
                        Message = "The checking account is inactive."
                    }
                };
            }
            catch (NegativeOrZeroMovementException)
            {
                return new CreateMovementAccountResponse
                {
                    Success = false,
                    Error = new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.INVALID_VALUE,
                        Message = "Only movements with positive values ​​can be registered."
                    }
                };
            }
            catch (MotionExceptionAboveAllowedValue)
            {
                return new CreateMovementAccountResponse
                {
                    Success = false,
                    Error = new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.INVALID_VALUE,
                        Message = "Only movements with values ​​up to 1.000.000 can be registered."
                    }
                };
            }
            catch (InvalidMovementTypeException)
            {
                return new CreateMovementAccountResponse
                {
                    Success = false,
                    Error = new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.INVALID_TYPE,
                        Message = "The movement type is invalid."
                    }
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<CreateMovementAccountResponse> TryGetIdempotentResult(CreateMovementAccountRequest request)
        {
            var idempotencyResult = await _idempotencyQueryRepository.GetIdempotency(request.IdempotencyKey);

            if (idempotencyResult != null)
            {
                try
                {
                    return new CreateMovementAccountResponse
                    {
                        Success = true,
                        Data = new CreateMovementAccountDto
                        {
                            MovementId = JsonConvert.DeserializeObject<string>(idempotencyResult)
                        }
                    };
                }
                catch (JsonException)
                {
                    return null;
                }
            }

            return null;
        }

        private void ValidateMovement(MovementAccount movement)
            => _movementValidationService.ValidadeState(movement);

        private void ValidateAccountStatus(AccountStatus statusAccount)
            => _accountValidationService.ValidadeAccountStatus(statusAccount);

        private void ValidadeExistingAccount(bool isAccount)
            => _accountValidationService.ValidadeExistingAccount(isAccount);

        private async Task CreateIdempotentEntry(CreateMovementAccountRequest request, string movementId)
        {
            var idempotency = new Idempotency
            {
                IdempotencyId = request.IdempotencyKey,
                Request = JsonConvert.SerializeObject(request),
                Result = JsonConvert.SerializeObject(movementId)
            };

            await _indepotencyCommandRepository.CreateIdempotency(idempotency);
        }
    }
}
