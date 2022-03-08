using Goal.Application.Seedwork.Handlers;
using Goal.Demo2.Api.Application.Commands.Customers;
using Goal.Demo2.Api.Application.Dtos.Customers.Requests;
using Goal.Demo2.Dto.Customers;
using Goal.Demo2.Infra.Data.Query.Repositories.Customers;
using Goal.Domain.Seedwork.Commands;
using Goal.Infra.Crosscutting.Adapters;
using Goal.Infra.Http.Seedwork.Controllers;
using Goal.Infra.Http.Seedwork.Controllers.Requests;
using Goal.Infra.Http.Seedwork.Controllers.Results;
using Goal.Infra.Http.Seedwork.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Goal.Demo2.Api.Controllers
{
    /// <summary>
    /// Everything about Customers
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ApiController
    {
        private readonly ICustomerQueryRepository customerRepository;
        private readonly IBusHandler busHandler;
        private readonly INotificationHandler notificationHandler;
        private readonly ITypeAdapter typeAdapter;

        public CustomersController(
            ICustomerQueryRepository customerRepository,
            IBusHandler busHandler,
            INotificationHandler notificationHandler,
            ITypeAdapter typeAdapter)
        {
            this.customerRepository = customerRepository;
            this.busHandler = busHandler;
            this.notificationHandler = notificationHandler;
            this.typeAdapter = typeAdapter;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<CustomerDto>>> Get([FromQuery] PaginationRequest request)
            => Paged(await customerRepository.QueryAsync(request.ToPagination()));

        [HttpGet("{id}", Name = nameof(GetById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerDto>> GetById([FromRoute] string id)
        {
            CustomerDto customer = await customerRepository.LoadAsync(id);

            if (customer is null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerDto>> Post([FromBody] RegisterNewCustomerRequest request)
        {
            var command = new RegisterNewCustomerCommand(
                request.Name,
                request.Email,
                request.BirthDate);

            ICommandResult<CustomerDto> result = await busHandler
                .SendCommand<RegisterNewCustomerCommand, CustomerDto>(command);

            if (result.IsValidationError())
            {
                return BadRequest();
            }

            if (result.IsDomainError())
            {
                return UnprocessableEntity();
            }

            return CreatedAtRoute(
                nameof(GetById),
                new { id = result.Data.CustomerId },
                result.Data);
        }

        [HttpPatch]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerDto>> Patch([FromRoute] Guid id, [FromBody] UpdateCustomerRequest request)
        {
            var command = new UpdateCustomerCommand(
                id,
                request.Name,
                request.BirthDate);

            ICommandResult result = await busHandler.SendCommand(command);

            if (result.IsValidationError())
            {
                return BadRequest();
            }

            if (result.IsDomainError())
            {
                return UnprocessableEntity();
            }

            return AcceptedAtAction(
                nameof(GetById),
                new { id },
                null);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            ICommandResult result = await busHandler.SendCommand(new RemoveCustomerCommand(id));

            if (result.IsValidationError())
            {
                return BadRequest();
            }

            if (result.IsDomainError())
            {
                return UnprocessableEntity();
            }

            return Accepted();
        }
    }
}
