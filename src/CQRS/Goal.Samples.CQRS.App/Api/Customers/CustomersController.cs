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

namespace Goal.Samples.CQRS.App.Api.Customers;

[ApiController]
[ApiVersion("1")]
[Authorize(Roles = "Administrator")]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse>> Get([FromQuery] PageSearchRequest request)
        => Paged(await customerQueryRepository.QueryAsync(request.ToPageSearch()));

    [HttpGet("{customerId}", Name = nameof(GetById))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
    public async Task<ActionResult<CustomerModel>> GetById([FromRoute] string customerId)
    {
        CustomerModel customer = await customerQueryRepository.LoadAsync(customerId);

        return customer is null
            ? NotFound()
            : Ok(customer);
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

        return !result.IsSucceeded
            ? CommandFailure(result)
            : CreatedAtRoute(
                nameof(GetById),
                new { customerId = result.Data.CustomerId },
                ApiResponse.FromCommand(result));
    }

    [HttpPatch("{customerId}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
    public async Task<ActionResult<ApiResponse<CustomerModel>>> Patch([FromRoute] string customerId, [FromBody] UpdateCustomerRequest request)
    {
        ICommandResult result = await mediator.Send(
            new UpdateCustomerCommand(
                customerId,
                request.Name,
                request.BirthDate));

        return result.IsSucceeded
            ? AcceptedAtAction(nameof(GetById), new { customerId }, null)
            : CommandFailure(result);
    }

    [HttpDelete("{customerId}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
    public async Task<ActionResult<ApiResponse>> Delete([FromRoute] string customerId)
    {
        ICommandResult result = await mediator.Send(new RemoveCustomerCommand(customerId));

        return result.IsSucceeded
            ? Accepted()
            : CommandFailure(result);
    }
}
