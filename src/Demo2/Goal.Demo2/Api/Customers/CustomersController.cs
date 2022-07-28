using Goal.Demo2.Application.Commands.Customers;
using Goal.Demo2.Infra.Data.Query.Repositories.Customers;
using Goal.Demo2.Model.Customers;
using Goal.Seedwork.Domain.Commands;
using Goal.Seedwork.Infra.Crosscutting.Notifications;
using Goal.Seedwork.Infra.Http.Controllers;
using Goal.Seedwork.Infra.Http.Controllers.Requests;
using Goal.Seedwork.Infra.Http.Controllers.Results;
using Goal.Seedwork.Infra.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Goal.Demo2.Api.Customers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ApiController
    {
        private readonly ICustomerQueryRepository customerRepository;
        private readonly IMediator mediator;
        private readonly INotificationHandler notificationHandler;

        public CustomersController(
            ICustomerQueryRepository customerRepository,
            IMediator mediator,
            INotificationHandler notificationHandler)
        {
            this.customerRepository = customerRepository;
            this.mediator = mediator;
            this.notificationHandler = notificationHandler;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<CustomerModel>>> Get([FromQuery] PaginationRequest request)
            => Paged(await customerRepository.QueryAsync(request.ToPagination()));

        [HttpGet("{id}", Name = nameof(GetById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerModel>> Post([FromBody] RegisterNewCustomerRequest request)
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
                    result.Data);
            }

            if (notificationHandler.HasInputValidation())
            {
                return BadRequest(notificationHandler.GetNotifications());
            }

            if (notificationHandler.HasDomainViolation())
            {
                return UnprocessableEntity(notificationHandler.GetNotifications());
            }

            if (notificationHandler.HasExternalError())
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, notificationHandler.GetNotifications());
            }

            return InternalServerError(notificationHandler.GetNotifications());
        }

        [HttpPatch]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerModel>> Patch([FromRoute] string id, [FromBody] UpdateCustomerRequest request)
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

            if (notificationHandler.HasInputValidation())
            {
                return BadRequest(notificationHandler.GetNotifications());
            }

            if (notificationHandler.HasDomainViolation())
            {
                return UnprocessableEntity(notificationHandler.GetNotifications());
            }

            if (notificationHandler.HasExternalError())
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, notificationHandler.GetNotifications());
            }

            return InternalServerError(notificationHandler.GetNotifications());
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete([FromRoute] string id)
        {
            ICommandResult result = await mediator.Send(new RemoveCustomerCommand(id));

            if (result.IsSucceeded)
            {
                return Accepted();
            }

            if (notificationHandler.HasInputValidation())
            {
                return BadRequest(notificationHandler.GetNotifications());
            }

            if (notificationHandler.HasDomainViolation())
            {
                return UnprocessableEntity(notificationHandler.GetNotifications());
            }

            if (notificationHandler.HasExternalError())
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, notificationHandler.GetNotifications());
            }

            return InternalServerError(notificationHandler.GetNotifications());
        }
    }
}
