using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Dtos;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Infrastructure.Filters;
using System.ComponentModel.DataAnnotations;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Movement registration.
        /// </summary>
        /// <remarks>
        /// Register a new transaction in an existing checking account.
        /// </remarks>
        /// <param name="createMovementAccountRequest">Data to create a new movement.</param>
        /// <returns></returns>
        /// <response code = "200">Returns the id of the created movement.</response>
        /// <response code = "400">Returns validation errors.</response>
        [ProducesResponseType(typeof(CreateMovementAccountDto), 200)]
        [ProducesResponseType(typeof(Response), 400)]  
        [HttpPost]
        [Route("createMovement")]
        [ValidateModelState]
        public async Task<ActionResult<CreateMovementAccountResponse>> Post([FromBody] CreateMovementAccountRequest createMovementAccountRequest)
        {
            var response = await _mediator.Send(createMovementAccountRequest);

            return response.Success ? (ActionResult<CreateMovementAccountResponse>)Ok(response)
                                    : (ActionResult<CreateMovementAccountResponse>)BadRequest(response);
        }

        /// <summary>
        /// Balance search.
        /// </summary>
        /// <remarks>
        /// Search the current account balance along with additional customer data.
        /// </remarks>
        /// <param name="id">Parameter to search for a current account.</param>
        /// <returns></returns>
        /// <response code = "200">Returns the request object.</response>
        /// <response code = "400">Returns validation errors.</response>
        [ProducesResponseType(typeof(CurrentAccountDto), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [HttpGet]
        [Route("getAmount")]
        public async Task<ActionResult<GetCurrentAccountResponse>> Get([FromQuery, Required] string id)
        {
            var getCurrentAccountRequest = new GetCurrentAccountRequest
            {
                AccountId = id
            };

            var response = await _mediator.Send(getCurrentAccountRequest);

            return response.Success ? (ActionResult<GetCurrentAccountResponse>)Ok(response)
                                    : (ActionResult<GetCurrentAccountResponse>)BadRequest(response);
        }
    }
}