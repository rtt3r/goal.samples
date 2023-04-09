using Goal.Samples.CQRS.Application.Commands.Customers;
using Goal.Samples.CQRS.Infra.Data.Query.Repositories.Customers;
using Goal.Samples.CQRS.Model.Customers;
using Goal.Samples.Infra.Http.Controllers;
using Goal.Seedwork.Application.Commands;
using Goal.Seedwork.Infra.Http.Controllers;
using Goal.Seedwork.Infra.Http.Controllers.Requests;
using Goal.Seedwork.Infra.Http.Controllers.Results;
using Goal.Seedwork.Infra.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Goal.Samples.CQRS.Api.Controllers.Customers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    public class CustomersController : ApiControllerBase
    {
        private readonly ICustomerQueryRepository customerQueryRepository;
        private readonly IMediator mediator;

        public CustomersController(
            ICustomerQueryRepository customerQueryRepository,
            IMediator mediator)
        {
            this.customerQueryRepository = customerQueryRepository;
            this.mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = "goal.read")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse>> Get([FromQuery] PageSearchRequest request)
            => Paged(await customerQueryRepository.QueryAsync(request.ToPageSearch()));

        [HttpGet("{id}", Name = nameof(GetById))]
        [Authorize(Policy = "goal.read")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        public async Task<ActionResult<CustomerModel>> GetById([FromRoute] string id)
        {
            CustomerModel customer = await customerQueryRepository.LoadAsync(id);

            if (customer is null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
        [Authorize(Policy = "goal.write")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
        public async Task<ActionResult<ApiResponse<CustomerModel>>> Post([FromBody] RegisterNewCustomerRequest request)
        {
            var command = new RegisterNewCustomerCommand(
                request.Name,
                request.Email,
                request.BirthDate);

            ICommandResult<CustomerModel> result = await mediator
                .Send<ICommandResult<CustomerModel>>(command);

            if (result.IsSucceeded)
            {
                return CreatedAtRoute(
                    nameof(GetById),
                    new { id = result.Data.CustomerId },
                    ApiResponse.FromCommand(result));
            }

            return CommandFailure(result);
        }

        [HttpPatch]
        [Route("{id}")]
        [Authorize(Policy = "goal.write")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
        public async Task<ActionResult<ApiResponse<CustomerModel>>> Patch([FromRoute] string id, [FromBody] UpdateCustomerRequest request)
        {
            var command = new UpdateCustomerCommand(
                id,
                request.Name,
                request.BirthDate);

            ICommandResult result = await mediator.Send(command);

            if (result.IsSucceeded)
            {
                return AcceptedAtAction(
                    nameof(GetById),
                    new { id },
                    null);
            }

            return CommandFailure(result);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = "goal.write")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
        public async Task<ActionResult<ApiResponse>> Delete([FromRoute] string id)
        {
            ICommandResult result = await mediator.Send(new RemoveCustomerCommand(id));

            if (result.IsSucceeded)
            {
                return Accepted();
            }

            return CommandFailure(result);
        }
    }
}
