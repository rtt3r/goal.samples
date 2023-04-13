using Goal.Seedwork.Infra.Crosscutting.Notifications;

namespace Goal.Samples.Infra.Http.Controllers;

public class ApiResponse<TData> : ApiResponse
{
    public ApiResponse(bool isSucceeded, TData data, params ApiResponseMessage[] messages)
        : base(isSucceeded, messages)
    {
        Data = data;
    }

    public ApiResponse(bool isSucceeded, TData data, params Notification[] notifications)
        : this(isSucceeded, data, notifications.Select(n => new ApiResponseMessage(n.Code, n.Message, n.ParamName)).ToArray())
    {
    }

    public TData Data { get; protected set; }
}
