using System.Net;
using System.Text.Json;
using Goal.Application.Seedwork.Handlers;
using Goal.Demo2.Api.Application.Commands.Customers;
using Goal.Demo2.Api.Application.Dtos.Customers.Requests;
using Goal.Demo2.Dto.Customers;
using Goal.Demo2.Infra.Data.Query.Repositories.Customers;
using Goal.Domain.Seedwork.Commands;
using Goal.Domain.Seedwork.Notifications;
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
        private readonly ILogger<CustomersController> logger;

        public CustomersController(
            ICustomerQueryRepository customerRepository,
            IBusHandler busHandler,
            INotificationHandler notificationHandler,
            ILogger<CustomersController> logger)
        {
            this.customerRepository = customerRepository;
            this.busHandler = busHandler;
            this.notificationHandler = notificationHandler;
            this.logger = logger;
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
            logger.LogTrace(
                "Executing action {ActionName} with payload: {Payload}",
                ControllerContext.ActionDescriptor.DisplayName,
                JsonSerializer.Serialize(request));

            var command = new RegisterNewCustomerCommand(
                request.Name,
                request.Email,
                request.BirthDate);

            logger.LogTrace(
                "Sending command {Name} to handler: {Command}",
                nameof(RegisterNewCustomerRequest),
                JsonSerializer.Serialize(command));

            ICommandResult<CustomerDto> result = await busHandler
                .SendCommand<RegisterNewCustomerCommand, CustomerDto>(command);

            logger.LogTrace(
                "Command handle result: {Result}",
                JsonSerializer.Serialize(result));

            if (result.IsValidationError())
            {
                IEnumerable<Notification> notifications = notificationHandler.GetNotifications();

                logger.LogTrace(
                    "Action result: {StatusCode} ({StatusCodeDescription}) => {Data}",
                    StatusCodes.Status400BadRequest,
                    $"{HttpStatusCode.BadRequest}",
                    JsonSerializer.Serialize(notifications));

                return BadRequest(notifications);
            }

            if (result.IsDomainError())
            {
                IEnumerable<Notification> notifications = notificationHandler.GetNotifications();

                logger.LogTrace(
                    "Action result: {StatusCode} ({StatusCodeDescription}) => {Data}",
                    StatusCodes.Status422UnprocessableEntity,
                    $"{HttpStatusCode.UnprocessableEntity}",
                    JsonSerializer.Serialize(notifications));

                return UnprocessableEntity(notificationHandler.GetNotifications());
            }

            logger.LogTrace(
                "Action result: {StatusCode} ({StatusCodeDescription}) => {Data}",
                StatusCodes.Status201Created,
                $"{HttpStatusCode.Created}",
                JsonSerializer.Serialize(result.Data));

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
