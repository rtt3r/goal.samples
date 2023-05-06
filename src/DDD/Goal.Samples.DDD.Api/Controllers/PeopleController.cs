using Goal.Samples.DDD.Application.DTO.People.Requests;
using Goal.Samples.DDD.Application.DTO.People.Responses;
using Goal.Samples.DDD.Application.People;
using Goal.Samples.Infra.Crosscutting.Exceptions;
using Goal.Seedwork.Infra.Http.Controllers;
using Goal.Seedwork.Infra.Http.Controllers.Requests;
using Goal.Seedwork.Infra.Http.Controllers.Results;
using Goal.Seedwork.Infra.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Goal.Samples.DDD.Api.Controllers;

/// <summary>
/// Everything about People
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public class PeopleController : ApiController
{
    private readonly IPersonAppService personAppService;

    public PeopleController(IPersonAppService personAppService)
    {
        this.personAppService = personAppService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<PersonResponse>>> Get([FromQuery] PageSearchRequest request)
        => Paged(await personAppService.FindPaginatedAsync(request.ToPageSearch()));

    [HttpGet("{id:Guid}", Name = nameof(GetById))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonResponse>> GetById(string id)
    {
        PersonResponse person = await personAppService.GetPersonAsync(id);

        return person is null
            ? NotFound()
            : Ok(person);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PersonResponse>> Post([FromBody] AddPersonRequest request)
    {
        return (await personAppService.AddPerson(request))
            .Match<ActionResult>(
                failure: ex => BadRequest(ex.Message),
                success: result => CreatedAtRoute(
                    nameof(GetById),
                    new { id = result.PersonId },
                    result));
    }

    [HttpPatch]
    [Route("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PersonResponse>> Patch(string id, [FromBody] UpdatePersonRequest request)
    {
        return (await personAppService.UpdatePerson(id, request))
            .Match<ActionResult>(
                failure: ex => ex is BusinessException
                    ? BadRequest(ex.Message)
                    : NotFound(ex.Message),
                success: result => AcceptedAtRoute(
                    nameof(GetById),
                    new { id = result.PersonId },
                    result));
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(string id)
    {
        return (await personAppService.DeletePerson(id))
            .Match<ActionResult>(
                failure: ex => NotFound(ex.Message),
                success: result => Accepted());
    }
}
