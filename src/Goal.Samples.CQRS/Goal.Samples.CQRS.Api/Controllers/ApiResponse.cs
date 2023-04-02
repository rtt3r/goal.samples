using Goal.Seedwork.Application.Commands;
using Goal.Seedwork.Infra.Crosscutting.Notifications;

namespace Goal.Samples.CQRS.Api.Controllers
{
    public class ApiResponse
    {
        public ApiResponse(bool isSucceeded, params ApiResponseMessage[] messages)
        {
            IsSucceeded = isSucceeded;
            Messages = messages ?? Array.Empty<ApiResponseMessage>();
        }

        public ApiResponse(bool isSucceeded, params Notification[] notifications)
            : this(isSucceeded, MapNotificationsToMessageArray(notifications))
        {
        }

        public bool IsSucceeded { get; private set; }
        public IEnumerable<ApiResponseMessage> Messages { get; private set; }

        public static ApiResponse Success()
            => new(true, Array.Empty<ApiResponseMessage>());

        public static ApiResponse<TData> Success<TData>(TData data)
            => new(true, data, Array.Empty<ApiResponseMessage>());

        public static ApiResponse Fail(IEnumerable<ApiResponseMessage> messages)
            => Fail(messages.ToArray());

        public static ApiResponse Fail(IEnumerable<Notification> notifications)
            => Fail(notifications.ToArray());

        public static ApiResponse Fail(params Notification[] notifications)
            => Fail(MapNotificationsToMessageArray(notifications));

        public static ApiResponse Fail(params ApiResponseMessage[] messages)
            => new(false, messages);

        public static ApiResponse FromCommand(ICommandResult result)
            => new(result.IsSucceeded, MapNotificationsToMessageArray(result.Notifications));

        public static ApiResponse<TData> FromCommand<TData>(ICommandResult<TData> result)
            => new(result.IsSucceeded, result.Data, MapNotificationsToMessageArray(result.Notifications));

        protected static ApiResponseMessage[] MapNotificationsToMessageArray(IEnumerable<Notification> notifications)
        {
            return (notifications ?? Enumerable.Empty<Notification>())
                .Select(n => new ApiResponseMessage(n.Code, n.Message, n.ParamName))
                .ToArray();
        }
    }

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

        public TData Data { get; private set; }
    }

    public class ApiResponseMessage
    {
        public string Code { get; private set; }
        public string Message { get; private set; }
        public string Param { get; private set; }

        public ApiResponseMessage(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public ApiResponseMessage(string code, string message, string param)
            : this(code, message)
        {
            Param = param;
        }
    }
}
