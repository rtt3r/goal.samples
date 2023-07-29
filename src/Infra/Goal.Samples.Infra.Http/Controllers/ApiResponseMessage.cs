namespace Goal.Samples.Infra.Http.Controllers;

public class ApiResponseMessage
{
    public string Code { get; protected set; }
    public string Message { get; protected set; }
    public string Param { get; protected set; }

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
