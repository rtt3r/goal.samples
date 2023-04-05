namespace Goal.Samples.Infra.Http.Controllers
{
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
