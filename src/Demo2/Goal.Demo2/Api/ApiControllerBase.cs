using Goal.Seedwork.Application.Commands;
using Goal.Seedwork.Infra.Http.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Goal.Demo2.Api
{
    public class ApiControllerBase : ApiController
    {
        protected override ActionResult CommandFailure(ICommandResult result)
        {
            if (result.HasInputValidation())
            {
                return BadRequest(ApiResponse.FromCommand(result));
            }

            if (result.HasResourceNotFound())
            {
                return NotFound(ApiResponse.FromCommand(result));
            }

            if (result.HasDomainViolation())
            {
                return UnprocessableEntity(ApiResponse.FromCommand(result));
            }

            if (result.HasExternalError())
            {
                return ServiceUnavailable(ApiResponse.FromCommand(result));
            }

            return InternalServerError(ApiResponse.FromCommand(result));
        }
    }
}
