using AutoMapper;
using MediatR;
using Questao5.Application.Dtos;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Application.Validators.Interfaces;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Infrastructure.Database.QueryStore.Requests.Interfaces;

namespace Questao5.Application.Handlers
{
    public class GetAccountHandler : IRequestHandler<GetCurrentAccountRequest, GetCurrentAccountResponse>
    {
        private readonly IAccountQueryRepository _accountQueryRepository;
        private readonly IGetAccountValidationService _accountValidationService;
        private readonly IMapper _mapper;

        public GetAccountHandler(IAccountQueryRepository accountQueryRepository,
                                 IGetAccountValidationService accountValidationService,
                                 IMapper mapper)
        {
            _accountQueryRepository = accountQueryRepository;
            _accountValidationService = accountValidationService;
            _mapper = mapper;
        }

        public async Task<GetCurrentAccountResponse> Handle(GetCurrentAccountRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var currentAccount = await _accountQueryRepository.GetCurrentAccountById(request.AccountId);

                _accountValidationService.ValidadeState(currentAccount);

                return new GetCurrentAccountResponse
                {
                    Success = true,
                    Data = _mapper.Map<CurrentAccountDto>(currentAccount)
                };
            }
            catch (AccountIsRequiredException)
            {
                return new GetCurrentAccountResponse
                {
                    Success = false,
                    Error = new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.INVALID_ACCOUNT,
                        Message = "Current account does not exist."
                    }
                };
            }
            catch (AccountIsInactiveException)
            {
                return new GetCurrentAccountResponse
                {
                    Success = false,
                    Error = new ErrorResponse
                    {
                        ErrorCode = ErrorCodes.INACTIVE_ACCOUNT,
                        Message = "Current account must be active."
                    }
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
