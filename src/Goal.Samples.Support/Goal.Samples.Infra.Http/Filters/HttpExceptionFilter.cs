using Goal.Samples.Infra.Crosscutting.Constants;
using Goal.Samples.Infra.Http.Controllers;
using Goal.Seedwork.Infra.Http.Controllers.Results;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Goal.Samples.Infra.Http.Filters;

public class HttpExceptionFilter : IExceptionFilter
{
    private readonly ILogger<HttpExceptionFilter> logger;

    public HttpExceptionFilter(ILogger<HttpExceptionFilter> logger)
    {
        this.logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        logger.LogError(context.Exception, ApplicationConstants.Messages.UNEXPECTED_ERROR);

        var response = ApiResponse.Fail(
            new ApiResponseMessage(
                nameof(ApplicationConstants.Messages.UNEXPECTED_ERROR),
                ApplicationConstants.Messages.UNEXPECTED_ERROR));

        context.Result = new InternalServerErrorObjectResult(response);
        context.ExceptionHandled = true;
    }
}
