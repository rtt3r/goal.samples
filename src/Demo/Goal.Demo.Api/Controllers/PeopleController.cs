using System.Threading.Tasks;
using Goal.Demo.Application.DTO.People.Requests;
using Goal.Demo.Application.DTO.People.Responses;
using Goal.Demo.Application.People;
using Goal.Infra.Http.Seedwork.Controllers;
using Goal.Infra.Http.Seedwork.Controllers.Requests;
using Goal.Infra.Http.Seedwork.Controllers.Results;
using Goal.Infra.Http.Seedwork.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Goal.Demo.Api.Controllers
{
    /// <summary>
    /// Everything about People
    /// </summary>
    [ApiController]
    //[ApiVersion("2")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    public class PeopleController : ApiController
    {
        private readonly IPersonAppService personAppService;

        public PeopleController(IPersonAppService personAppService)
        {
            this.personAppService = personAppService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<PersonResponse>>> Get([FromQuery] PaginationRequest request)
            => Paged(await personAppService.FindPaginatedAsync(request.ToPagination()));

        [HttpGet("{id:Guid}", Name = nameof(GetById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PersonResponse>> GetById(string id)
        {
            PersonResponse person = await personAppService.GetPersonAsync(id);

            if (person is null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonResponse>> Post([FromBody] AddPersonRequest request)
        {
            PersonResponse result = await personAppService.AddPerson(request);

            if (result is null)
            {
                return BadRequest();
            }

            return CreatedAtRoute(
                nameof(GetById),
                new { id = result.PersonId },
                result);
        }

        [HttpPatch]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonResponse>> Patch(string id, [FromBody] UpdatePersonRequest request)
        {
            PersonResponse result = await personAppService.UpdatePerson(id, request);

            if (result is null)
            {
                return BadRequest();
            }

            return AcceptedAtAction(
                nameof(GetById),
                new { id = result.PersonId },
                result);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(string id)
        {
            bool result = await personAppService.DeletePerson(id);

            if (result)
            {
                return NotFound();
            }

            return Accepted();
        }
    }
}
