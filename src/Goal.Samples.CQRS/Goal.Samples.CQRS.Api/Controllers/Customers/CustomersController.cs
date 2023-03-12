using Goal.Samples.CQRS.Application.Commands.Customers;
using Goal.Samples.CQRS.Infra.Data.Query.Repositories.Customers;
using Goal.Samples.CQRS.Model.Customers;
using Goal.Seedwork.Application.Commands;
using Goal.Seedwork.Infra.Http.Controllers;
using Goal.Seedwork.Infra.Http.Controllers.Requests;
using Goal.Seedwork.Infra.Http.Controllers.Results;
using Goal.Seedwork.Infra.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Goal.Samples.CQRS.Api.Controllers.Customers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ApiControllerBase
    {
        private readonly ICustomerQueryRepository customerRepository;
        private readonly IMediator mediator;

        public CustomersController(
            ICustomerQueryRepository customerRepository,
            IMediator mediator)
        {
            this.customerRepository = customerRepository;
            this.mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<CustomerModel>>> Get([FromQuery] PageSearchRequest request)
            => Paged(await customerRepository.QueryAsync(request.ToPageSearch()));

        [HttpGet("{id}", Name = nameof(GetById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        public async Task<ActionResult<CustomerModel>> GetById([FromRoute] string id)
        {
            CustomerModel customer = await customerRepository.LoadAsync(id);

            if (customer is null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
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
